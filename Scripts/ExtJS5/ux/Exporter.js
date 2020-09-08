Ext.define("Ext.ux.exporter.Formatter", {
    format: Ext.emptyFn,
    constructor: function (config) {
        config = config || {};
        Ext.applyIf(config, {
        });
    }
});

Ext.define('Ext.ux.exporter.CsvFormarter', {
    extend: "Ext.ux.exporter.Formatter",
    separator: ";",
    extension: "csv",

    format: function (grid) {
        return this.getHeaders(grid) + "\n" + this.getRows(grid);
    },

    getHeaders: function (grid) {
        var cols = [];
        Ext.each(grid.columns, function (column) {
            if (column.dataIndex)
                cols.push(Ext.String.capitalize(column.text.replace(/_/g, " ")));
        }, this);
        return cols.join(this.separator);
    },

    getRows: function (grid) {
        var rows = [];
        grid.store.each(function (record, index) {
            var cells = [];
            Ext.each(grid.columns, function (col) {
                var name = col.dataIndex;
                if (name && col.hidden != true) {
                    if (Ext.isFunction(col.renderer)) {
                        var value = col.renderer(record.get(name), null, record);
                    } else {
                        var value = record.get(name);
                    }
                    cells.push(value);
                }
            }, this);
            rows.push(cells.join(this.separator));
        }, this);
        return rows.join("\n");
    }
});

Ext.define('Ext.ux.exporter.Button', {
    extend: 'Ext.button.Button',
    alias: 'widget.exporterbutton',

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    },

    onClick: function (e) {
        var me = this;
        var form = Ext.getDom('export');
        if (!form) {
            form = Ext.DomHelper.append(Ext.getBody(), {
                tag: 'form',
                method: 'post',
                id: 'export',
                action: me.action,
                style: "display: none;",
                cn: [{
                    tag: 'input',
                    name: 'f',
                    type: 'hidden'
                }, {
                    tag: 'input',
                    name: 'd',
                    type: 'hidden'
                }]
            });
        }
        var grid = me.up('grid');
        //var cvs = Ext.create('Ext.ux.exporter.CsvFormarter');
        var cvs = Ext.create('Ext.ux.exporter.ExcelFormatter');
        form.f.value = me.file;
        form.d.value = cvs.format(grid);
        form.submit();
        form.d.value = '';
        Ext.ux.exporter.Button.superclass.onClick.call(this, e);
    }
});

Ext.define("Ext.ux.exporter.excelFormatter.Style", {
    constructor: function (config) {
        config = config || {};

        Ext.apply(this, config, {
            parentStyle: '',
            attributes: []
        });

        Ext.ux.exporter.excelFormatter.Style.superclass.constructor.apply(this, arguments);

        if (this.id == undefined) throw new Error("An ID must be provided to Style");

        this.preparePropertyStrings();
    },

    /**
     * Iterates over the attributes in this style, and any children they may have, creating property
     * strings on each suitable for use in the XTemplate
     */
    preparePropertyStrings: function () {
        Ext.each(this.attributes, function (attr, index) {
            this.attributes[index].propertiesString = this.buildPropertyString(attr);
            this.attributes[index].children = attr.children || [];

            Ext.each(attr.children, function (child, childIndex) {
                this.attributes[index].children[childIndex].propertiesString = this.buildPropertyString(child);
            }, this);
        }, this);
    },

    /**
     * Builds a concatenated property string for a given attribute, suitable for use in the XTemplate
     */
    buildPropertyString: function (attribute) {
        var propertiesString = "";

        Ext.each(attribute.properties || [], function (property) {
            propertiesString += Ext.String.format('ss:{0}="{1}" ', property.name, property.value);
        }, this);

        return propertiesString;
    },

    render: function () {
        return this.tpl.apply(this);
    },

    tpl: new Ext.XTemplate(
      '<tpl if="parentStyle.length == 0">',
        '<ss:Style ss:ID="{id}">',
      '</tpl>',
      '<tpl if="parentStyle.length != 0">',
        '<ss:Style ss:ID="{id}" ss:Parent="{parentStyle}">',
      '</tpl>',
      '<tpl for="attributes">',
        '<tpl if="children.length == 0">',
          '<ss:{name} {propertiesString}></ss:{name}>',
        '</tpl>',
        '<tpl if="children.length &gt; 0">',
          '<ss:{name} {propertiesString}>',
            '<tpl for="children">',
              '<ss:{name} {propertiesString} />',
            '</tpl>',
          '</ss:{name}>',
        '</tpl>',
      '</tpl>',
      '</ss:Style>'
    )
});

Ext.define("Ext.ux.exporter.excelFormatter.Workbook", {

    constructor: function (config) {
        config = config || {};

        Ext.apply(this, config, {
            /**
             * @property title
             * @type String
             * The title of the workbook (defaults to "Workbook")
             */
            title: "Workbook",

            /**
             * @property worksheets
             * @type Array
             * The array of worksheets inside this workbook
             */
            worksheets: [],

            /**
             * @property compileWorksheets
             * @type Array
             * Array of all rendered Worksheets
             */
            compiledWorksheets: [],

            /**
             * @property cellBorderColor
             * @type String
             * The colour of border to use for each Cell
             */
            cellBorderColor: "#e4e4e4",

            /**
             * @property styles
             * @type Array
             * The array of Ext.ux.Exporter.ExcelFormatter.Style objects attached to this workbook
             */
            styles: [],

            /**
             * @property compiledStyles
             * @type Array
             * Array of all rendered Ext.ux.Exporter.ExcelFormatter.Style objects for this workbook
             */
            compiledStyles: [],

            /**
             * @property hasDefaultStyle
             * @type Boolean
             * True to add the default styling options to all cells (defaults to true)
             */
            hasDefaultStyle: true,

            windowHeight: 9000,
            windowWidth: 50000,
            protectStructure: false,
            protectWindows: false
        });

        if (this.hasDefaultStyle) this.addDefaultStyle();
        this.addHeaderStyle();
    },

    render: function () {
        this.compileStyles();
        this.joinedCompiledStyles = this.compiledStyles.join("");

        this.compileWorksheets();
        this.joinedWorksheets = this.compiledWorksheets.join("");

        return this.tpl.apply(this);
    },

    /**
     * Adds a worksheet to this workbook based on a store and optional config
     * @param {Ext.data.Store} store The store to initialize the worksheet with
     * @param {Object} config Optional config object
     * @return {Ext.ux.Exporter.ExcelFormatter.Worksheet} The worksheet
     */
    addWorksheet: function (grid, config) {
        var worksheet = new Ext.ux.exporter.excelFormatter.Worksheet(grid /*store*/, config);

        this.worksheets.push(worksheet);

        return worksheet;
    },

    /**
     * Adds a new Ext.ux.Exporter.ExcelFormatter.Style to this Workbook
     * @param {Object} config The style config, passed to the Style constructor (required)
     */
    addStyle: function (config) {
        var style = new Ext.ux.exporter.excelFormatter.Style(config || {});

        this.styles.push(style);

        return style;
    },

    /**
     * Compiles each Style attached to this Workbook by rendering it
     * @return {Array} The compiled styles array
     */
    compileStyles: function () {
        this.compiledStyles = [];

        Ext.each(this.styles, function (style) {
            this.compiledStyles.push(style.render());
        }, this);

        return this.compiledStyles;
    },

    /**
     * Compiles each Worksheet attached to this Workbook by rendering it
     * @return {Array} The compiled worksheets array
     */
    compileWorksheets: function () {
        this.compiledWorksheets = [];

        Ext.each(this.worksheets, function (worksheet) {
            this.compiledWorksheets.push(worksheet.render());
        }, this);

        return this.compiledWorksheets;
    },

    tpl: new Ext.XTemplate(
      '<?xml version="1.0" encoding="utf-8"?>',
      '<?mso-application progid="Excel.Sheet"?>',
      '<ss:Workbook xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:o="urn:schemas-microsoft-com:office:office">',
        '<o:DocumentProperties>',
          '<o:Title>{title}</o:Title>',
        '</o:DocumentProperties>',
        '<ss:ExcelWorkbook>',
          '<ss:WindowHeight>{windowHeight}</ss:WindowHeight>',
          '<ss:WindowWidth>{windowWidth}</ss:WindowWidth>',
          '<ss:ProtectStructure>{protectStructure}</ss:ProtectStructure>',
          '<ss:ProtectWindows>{protectWindows}</ss:ProtectWindows>',
        '</ss:ExcelWorkbook>',
        '<ss:Styles>',
          '{joinedCompiledStyles}',
        '</ss:Styles>',
          '{joinedWorksheets}',
      '</ss:Workbook>'
    ),

    /**
     * Adds the default Style to this workbook. This sets the default font face and size, as well as cell borders
     */
    addDefaultStyle: function () {
        var borderProperties = [
          { name: "Color", value: this.cellBorderColor },
          { name: "Weight", value: "1" },
          { name: "LineStyle", value: "Continuous" }
        ];

        this.addStyle({
            id: 'Default',
            attributes: [
              {
                  name: "Alignment",
                  properties: [
                    { name: "Vertical", value: "Top" },
                    { name: "WrapText", value: "0" }
                  ]
              },
              {
                  name: "Font",
                  properties: [
                    { name: "FontName", value: "arial" },
                    { name: "Size", value: "8" }
                  ]
              },
              { name: "Interior" }, { name: "NumberFormat" }, { name: "Protection" },

              {
                  name: "Borders",
                  children: [
                    {
                        name: "Border",
                        properties: [{ name: "Position", value: "Top" }].concat(borderProperties)
                    },
                    {
                        name: "Border",
                        properties: [{ name: "Position", value: "Bottom" }].concat(borderProperties)
                    },
                    {
                        name: "Border",
                        properties: [{ name: "Position", value: "Left" }].concat(borderProperties)
                    },
                    {
                        name: "Border",
                        properties: [{ name: "Position", value: "Right" }].concat(borderProperties)
                    }
                  ]
              }
            ]
        });

        this.addStyle({
            id: "dt",
            attributes: [
              {
                  name: "NumberFormat",
                  properties: [{ name: "Format", value: "Short Date" }]
              }
            ]
        });

        this.addStyle({
            id: "nm",
            attributes: [
              {
                  name: "NumberFormat",
                  properties: [{ name: "Format", value: "Standard" }]
              }
            ]
        });
    },

    addHeaderStyle: function () {
        this.addStyle({
            id: "headercell",
            attributes: [
              {
                  name: "Font",
                  properties: [
                    { name: "Bold", value: "1" },
                    { name: "Size", value: "8" }
                  ]
              },
              {
                  name: "Interior",
                  properties: [
                    { name: "Pattern", value: "Solid" },
                    { name: "Color", value: "#A3C9F1" }
                  ]
              },
              {
                  name: "Alignment",
                  properties: [
                    { name: "WrapText", value: "1" },
                    { name: "Horizontal", value: "Center" }
                  ]
              }
            ]
        });
    }

});

Ext.define("Ext.ux.exporter.excelFormatter.Worksheet", {

    constructor: function (grid, config) {
        config = config || {};

        this.grid = grid;

        Ext.applyIf(config, {
            hasHeadings: true,

            title: "Лист"//,
            //columns: store.fields == undefined ? {} : store.fields.items
        });

        Ext.apply(this, config);

        Ext.ux.exporter.excelFormatter.Worksheet.superclass.constructor.apply(this, arguments);
    },

    /**
     * @property dateFormatString
     * @type String
     * String used to format dates (defaults to "Y-m-d"). All other data types are left unmolested
     */
    dateFormatString: "Y-m-d",

    worksheetTpl: new Ext.XTemplate(
      '<ss:Worksheet ss:Name="{title}">',
        '<ss:Names>',
          '<ss:NamedRange ss:Name="Print_Titles" ss:RefersTo="=\'{title}\'!R1:R2" />',
        '</ss:Names>',
        '<ss:Table x:FullRows="1" x:FullColumns="1" ss:ExpandedColumnCount="{colCount}" ss:ExpandedRowCount="{rowCount}">',
          '{columns}',
          '<ss:Row ss:AutoFitHeight="1">',
            '{header}',
          '</ss:Row>',
          '{rows}',
        '</ss:Table>',
        '<x:WorksheetOptions>',
          '<x:PageSetup>',
            '<x:Layout x:CenterHorizontal="1" x:Orientation="Landscape" />',
            '<x:Footer x:Data="Page &amp;P of &amp;N" x:Margin="0.5" />',
            '<x:PageMargins x:Top="0.5" x:Right="0.5" x:Left="0.5" x:Bottom="0.8" />',
          '</x:PageSetup>',
          '<x:FitToPage />',
          '<x:Print>',
            '<x:PrintErrors>Blank</x:PrintErrors>',
            '<x:FitWidth>1</x:FitWidth>',
            '<x:FitHeight>32767</x:FitHeight>',
            '<x:ValidPrinterInfo />',
            '<x:VerticalResolution>600</x:VerticalResolution>',
          '</x:Print>',
          '<x:Selected />',
          '<x:DoNotDisplayGridlines />',
          '<x:ProtectObjects>False</x:ProtectObjects>',
          '<x:ProtectScenarios>False</x:ProtectScenarios>',
        '</x:WorksheetOptions>',
      '</ss:Worksheet>'
    ),

    /**
     * Builds the Worksheet XML
     * @param {Ext.data.Store} store The store to build from
     */
    render: function (store) {
        return this.worksheetTpl.apply({
            header: this.buildHeader(),
            columns: this.buildColumns().join(""),
            rows: this.buildRows().join(""),
            colCount: this.grid.columns.length,
            rowCount: this.grid.store.getCount() + 2,
            title: this.title
        });
    },

    buildColumns: function () {
        var cols = [];

        Ext.each(this.grid.columns, function (column) {
            if (column.hidden != true)
                cols.push(this.buildColumn());
        }, this);

        return cols;
    },

    buildColumn: function (width) {
        return Ext.String.format('<ss:Column ss:AutoFitWidth="1" />');
        //return Ext.String.format('<ss:Column ss:AutoFitWidth="1" ss:Width="{0}" />', width || 164);
    },

    buildRows: function () {
        var rows = [];

        this.grid.store.each(function (record, index) {
            rows.push(this.buildRow(record, index));
        }, this);

        return rows;
    },

    buildHeader: function () {
        var cells = [];
        Ext.each(this.grid.columns, function (col) {
            var title;
            if (col.dataIndex && col.hidden != true) {
                title = Ext.String.capitalize(col.text.replace(/_/g, " "));
                cells.push(Ext.String.format('<ss:Cell ss:StyleID="headercell"><ss:Data ss:Type="String">{0}</ss:Data><ss:NamedCell ss:Name="Print_Titles" /></ss:Cell>', title));
            }
        }, this);
        return cells.join("");
    },

    buildRow: function (record, index) {
        var style, cells = [];

        Ext.each(this.grid.columns, function (col) {
            var name = col.dataIndex;

            if (name && col.hidden != true) {
                //if given a renderer via a ColumnModel, use it and ensure data type is set to String
                if (col.xtype == "checkcolumn") {
                    var value = (record.get(name) == true ? 'Да' : 'Нет'),
                    type = this.typeMappings[record.getField(name).type || col.xtype || col.type];
                }
                else if (Ext.isFunction(col.renderer) && col.xtype != "numbercolumn" && record.getField(name).type != "date") {
                    var value = col.renderer(record.get(name), null, record),
                        type = record.getField(name).type == "date" ? "DateTime" : "String";
                } else {
                    var value = record.get(name),
                        type = this.typeMappings[record.getField(name).type || col.xtype || col.type];
                }

                cells.push(this.buildCell(value, type, style).render());
            }
        }, this);

        return Ext.String.format("<ss:Row>{0}</ss:Row>", cells.join(""));
    },

    buildCell: function (value, type, style) {
        if (type == "DateTime" && value && typeof value != "string")
            value = Ext.Date.toString(value);

        return new Ext.ux.exporter.excelFormatter.Cell({
            value: value,
            type: type,
            style: type == "DateTime" ? "dt" : type == "Number" ? "nm" : style
        });
    },

    /**
     * @property typeMappings
     * @type Object
     * Mappings from Ext.data.Record types to Excel types
     */
    typeMappings: {
        'int': "Number",
        'string': "String",
        'float': "Number",
        'date': "DateTime",
        'numbercolumn': "Number",
        'datecolumn': "DateTime"
    }
});

Ext.define("Ext.ux.exporter.excelFormatter.Cell", {
    constructor: function (config) {
        Ext.applyIf(config, {
            type: "String"
        });

        Ext.apply(this, config);

        Ext.ux.exporter.excelFormatter.Cell.superclass.constructor.apply(this, arguments);
    },

    render: function () {
        return this.tpl.apply(this);
    },

    tpl: new Ext.XTemplate(
        '<tpl if="style != null">',
        '<ss:Cell ss:StyleID="{style}">',
        '</tpl>',
        '<tpl if="style == null">',
        '<ss:Cell>',
        '</tpl>',
        '<tpl if="value != null">',
          '<ss:Data ss:Type="{type}"><![CDATA[{value}]]></ss:Data>',
        '</tpl>',
        '</ss:Cell>'
    )
});

Ext.define("Ext.ux.exporter.ExcelFormatter", {
    extend: "Ext.ux.exporter.Formatter",
    uses: [
        "Ext.ux.exporter.excelFormatter.Cell",
        "Ext.ux.exporter.excelFormatter.Style",
        "Ext.ux.exporter.excelFormatter.Worksheet",
        "Ext.ux.exporter.excelFormatter.Workbook"
    ],
    contentType: 'data:application/vnd.ms-excel;base64,',
    extension: "xls",

    format: function (grid, config) {
        var workbook = new Ext.ux.exporter.excelFormatter.Workbook(config);
        workbook.addWorksheet(grid, config || {});

        return workbook.render();
    }
});
