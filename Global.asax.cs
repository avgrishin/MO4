using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MO
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      //RouteTable.Routes.MapHubs();
      AutoMapper.Mapper.CreateMap<MO.Models.tRiskEvent, MO.Models.tRiskEvent>()
        .ForMember(dest => dest.ID, opt => opt.Ignore())
        .ForMember(dest => dest.InDateTime, opt => opt.Ignore())
        .ForMember(dest => dest.Date1, opt => opt.MapFrom(src => src.Date1.HasValue ? src.Date1.Value.AddHours(1).Date : src.Date1))
        .ForMember(dest => dest.Date2, opt => opt.MapFrom(src => src.Date2.HasValue ? src.Date2.Value.AddHours(1).Date : src.Date2))
        .ForMember(dest => dest.Date3, opt => opt.MapFrom(src => src.Date3.HasValue ? src.Date3.Value.AddHours(1).Date : src.Date3))
        .ForMember(dest => dest.Date4, opt => opt.MapFrom(src => src.Date4.HasValue ? src.Date4.Value.AddHours(1).Date : src.Date4))
        .AfterMap((src, dest) => dest.InDateTime = DateTime.Now
        );
      AutoMapper.Mapper.CreateMap<MO.Models.tRegDoc, MO.Models.tRegDoc>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.InDateTime, opt => opt.Ignore())
        .ForMember(dest => dest.UserName, opt => opt.Ignore());
      AutoMapper.Mapper.CreateMap<MO.Models.tOutgoing, MO.Models.tOutgoing>()
        .ForMember(dest => dest.Id, opt => opt.Ignore())
        .ForMember(dest => dest.InDateTime, opt => opt.Ignore())
        .ForMember(dest => dest.UserName, opt => opt.Ignore());
      AutoMapper.Mapper.CreateMap<MO.Areas.Code.Models.LeaveViewModel, MO.Models.tLeave>();
      AutoMapper.Mapper.CreateMap<MO.Models.tOrdPayment1, MO.Models.tOrdPayment>();
      AutoMapper.Mapper.CreateMap<MO.Models.tOrdPaymentDet, MO.Models.tOrdPaymentDet>();
      AutoMapper.Mapper.CreateMap<MO.Models.tContragent, MO.Models.tContragent>();
      AutoMapper.Mapper.CreateMap<MO.Models.tContragentDog, MO.Models.tContragentDog>();
      AutoMapper.Mapper.CreateMap<MO.Models.tDiv, MO.Models.tDiv>();
      AreaRegistration.RegisterAllAreas();
      AjaxHelper.GlobalizationScriptPath = "http://ajax.microsoft.com/ajax/4.0/1/globalization/";
      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
  }
}