using applyRequests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace applyRequests.Controllers
{
    public class UserAPIController : ApiController
    {
        controlDoActioncs controlObj = new controlDoActioncs();

        // GET api/<controller>
        public IEnumerable<flowRole> Get(string userListType,string userID)
        {
            try
            {
                //傳回RD處理人員(可接受任務指派的人員)
                if (userListType == "rdUsers")
                {
                    return controlObj.listRdAcceptTaskUsers();
                }
                else
                {
                    //傳回所有使用者列表
                    return controlObj.listAllUsers(userID);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }


        public List<string> Get()
        {
            try
            {
                List<string> liStrSamples = new List<string>();
                liStrSamples.Add("AAA");
                liStrSamples.Add("bbb");
                liStrSamples.Add("ccc");
                return liStrSamples;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}