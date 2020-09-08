using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MO.Models;

namespace MO.Controllers
{
  public class RepWAController : ApiController
  {
    private IReportRepository repRepository;

    public RepWAController(IReportRepository repRepo)
    {
      repRepository = repRepo;
    }

    [Authorize]
    //[HttpGet]
    public IEnumerable<dynamic> GetData(int id)
    {

      if (id == 1)
        return repRepository.GetValRates();
      else if (id == 2)
        return repRepository.GetCourses();
      else if (id == 3)
        return repRepository.GetPifRates();
      else if (id == 4)
        return repRepository.GetBotPifRates();
      else if (id == 5)
        return repRepository.GetBotPifYield();
      else if (id == 6)
        return repRepository.GetBotPifBranch();
      else if (id == 7)
        return repRepository.GetBotDuBranch();
      else
        throw new HttpResponseException(HttpStatusCode.NotFound);
    }

    //public class PifSeacher { public string q { get; set; } }
    //[HttpPost]
    public IEnumerable<dynamic> PostPifers([FromBody]string q)
    {
      return repRepository.GetPifers(q);
    }

    public class PiferRestParams { public int? id { get; set; } public DateTime? d1 { get; set; } public DateTime? d2 { get; set; } }
    public object PostPiferRest([FromBody]PiferRestParams data)
    {
      if (data.id.HasValue)
        return repRepository.GetPiferRest(data.id.Value, data.d1, data.d2);
      return BadRequest("Не задан параметер");
    }

    public class PiferYieldParams { public int id { get; set; } public DateTime? d1 { get; set; } public DateTime? d2 { get; set; } }

    public object PostPiferYield([FromBody]PiferYieldParams data)
    {
      return repRepository.GetPiferYield(data.id, data.d1, data.d2);
    }

    public class PifOrdersParams { public int? id { get; set; } public int? secId { get; set; } public DateTime? d1 { get; set; } public DateTime? d2 { get; set; } }

    public object PostPiferFondYield(PifOrdersParams param)
    {
      if (param.id.HasValue && param.secId.HasValue)
        return repRepository.GetPiferFondYield(param.id.Value, param.secId.Value, param.d1, param.d2);
      return BadRequest("Не задан параметер");
    }

    public object PostPiferOrders(PifOrdersParams param)
    {
      if (param.id.HasValue && param.secId.HasValue)
        return repRepository.GetPiferOrders(param.id.Value, param.secId.Value);
      return BadRequest("Не задан параметер");
    }

    public object GetPiferGraph(int? id)
    {
      if (!id.HasValue)
        return BadRequest("Не задан параметер");
      return repRepository.GetPiferGraph(id.Value);
    }

    public object PostPiferGraph3([FromBody]PiferYieldParams data)
    {
      if (data.d1.HasValue)
        data.d1 = data.d1.Value.ToLocalTime();
      if (data.d2.HasValue)
        data.d2 = data.d2.Value.ToLocalTime();
      return repRepository.GetPiferGraph3(data.id, data.d1, data.d2);
    }

    //[Authorize(Roles = "bank, mo, bank1, PIF")]
    //public IEnumerable<dynamic> GetCourses()
    //{
    //  return repRepository.GetCourses();
    //}
  }
}
