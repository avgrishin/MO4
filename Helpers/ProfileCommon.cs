using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;

namespace MO.Helpers
{
  public class ProfileCommon : ProfileBase
  {
    public virtual DateTime? db { 
      get 
      { 
        return ((DateTime?)(this.GetPropertyValue("db"))); 
      } 
      set 
      { 
        this.SetPropertyValue("db", value); 
      } 
    }

    public virtual DateTime? de
    {
      get
      {
        return ((DateTime?)(this.GetPropertyValue("de")));
      }
      set
      {
        this.SetPropertyValue("de", value);
      }
    }

    public virtual int? FundID
    {
      get
      {
        return ((int?)(this.GetPropertyValue("FundID")));
      }
      set
      {
        this.SetPropertyValue("FundID", value);
      }
    }

    public virtual int? FinInstID
    {
      get
      {
        return ((int?)(this.GetPropertyValue("FinInstID")));
      }
      set
      {
        this.SetPropertyValue("FinInstID", value);
      }
    }

    public virtual int? IndexID
    {
      get
      {
        return ((int?)(this.GetPropertyValue("IndexID")));
      }
      set
      {
        this.SetPropertyValue("IndexID", value);
      }
    }
    public virtual int? StrategyID
    {
      get
      {
        return ((int?)(this.GetPropertyValue("StrategyID")));
      }
      set
      {
        this.SetPropertyValue("StrategyID", value);
      }
    }
  }
}
