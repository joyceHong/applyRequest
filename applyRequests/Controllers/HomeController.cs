using applyRequests.Models;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace applyRequests.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.Default)]
    public class HomeController : Controller
    {
        WriteEvent.WrittingEventLog writeObj = new WriteEvent.WrittingEventLog();
        string path = AppDomain.CurrentDomain.BaseDirectory;

        TCSNewEntities entityTCS = new TCSNewEntities();
        entityRequest entityRquestObj = new entityRequest();
        controlDoActioncs controlObj = new controlDoActioncs();
        
        public ActionResult login()
        {
            Session["uid"] = "";
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult login(string userAccount, string userPassword)
        {
            if (controlObj.loginPass(userAccount, userPassword)==1)
            {
                Session["uid"] = userAccount;
                Response.Redirect("listRequests");
            }
            else
            {
                ViewData["msg"] = "帳號/密碼 輸入錯誤";
            }
            return View();
        }
      
        public ActionResult applyRequest()
        {
            ViewData["viewApplyUser"] = Session["uid"];
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult applyRequest(string applyUser, DateTime applyDate,string applyTitle, string applyReason)
        {
            try
            {
                ViewBag.Message = "Your application description page.";
                controlObj.saveApplyRequest(applyUser, applyDate, applyTitle, applyReason);
                ViewData["page"] = "listRequests";
                Response.Redirect("listRequests");
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error"+DateTime.Today.ToString("yyyyMMdd"), path, "applyRequest:" + ex.Message);
                return View();                
            }
        }
        
        public ActionResult listRequests()
        {
            try
            {
                ViewData["page"] = "listRequests";
                ViewData["viewProcessAction"] = "";
                ViewData["viewApplyReason"] = "";
                ViewData["startDate"] = null;
                ViewData["endDate"] = null;

                if (Session["uid"].ToString().Trim() == "")
                {
                    Response.Redirect("login");
                }

                string strUid = Session["uid"].ToString();

                if (string.IsNullOrEmpty(strUid))
                {
                    Response.Redirect("login");
                }

                return View(entityRquestObj.listRequests(strUid, null, null, "", ""));
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error"+DateTime.Today.ToString("yyyyMMdd"), path, "listRequests:" + ex.Message);
                return View(); 
            }
           
        }
        
        private List<ProcessOption> liProActionOptions()
        {
            List<ProcessOption> liProcessOptions = new List<ProcessOption>();
            liProcessOptions.Add(new ProcessOption()
            {
                name = "審核中",
                value = "other"
            });

            liProcessOptions.Add(new ProcessOption()
            {
                name = "審核通過",
                value = "complete"
            });

            liProcessOptions.Add(new ProcessOption()
            {
                name = "完工",
                value = "end"
            });

            liProcessOptions.Add(new ProcessOption()
            {
                name = "審核退回",
                value = "reject"
            });

            return liProcessOptions;
        }

        [HttpPost]
        public ActionResult listRequests( DateTime? startDate, DateTime? endDate, string processAction, string applyReason)
        {
            try
            {

                ViewData["page"] = "listRequests";
                ViewData["viewProcessAction"] = processAction;
                ViewData["viewApplyReason"] = applyReason;
                ViewData["startDate"] = startDate;
                ViewData["endDate"] = endDate;

                string strUid = sessionUID(startDate, endDate);                
                return View(entityRquestObj.listRequests(strUid,startDate,endDate,processAction,applyReason));
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "listRequests Post:" + ex.Message);
                return View();  
            }
        }

        private string sessionUID(DateTime? startDate, DateTime? endDate)
        {
            if (Session["uid"] == null)
            {
                Response.Redirect("login");
            }

            string strUid = Session["uid"].ToString();

            if (string.IsNullOrEmpty(strUid))
            {
                Response.Redirect("login");
            }

            if (startDate != null && endDate != null)
            {
                ViewData["startDate"] = startDate.Value.ToString("yyyy/MM/dd");
                ViewData["endDate"] = endDate.Value.ToString("yyyy/MM/dd");
            }
            return strUid;
        }

        public ActionResult listWaitToSignRequests()
        {
            try
            {
                ViewData["page"] = "listWaitToSignRequests";
                if (Session["uid"].ToString().Trim() == "")
                {
                    Response.Redirect("login");
                }

                string strUid = Session["uid"].ToString();
                return View(entityRquestObj.listWaitToSignRequests(strUid, null, null, "")); 
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "listWaitToSignRequests:" + ex.Message);
                return View();  
            }
        }

        [HttpPost]
        public ActionResult listWaitToSignRequests(DateTime? startDate, DateTime? endDate, string processAction, string applyReason)
        {
            try
            {
                ViewData["page"] = "listWaitToSignRequests";
                string strUid = sessionUID(startDate, endDate);
                return View(entityRquestObj.listWaitToSignRequests(strUid, startDate, endDate, applyReason));   
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "listWaitToSignRequests Post:" + ex.Message);
                return View();  
            }
        }
        
        public ActionResult listSignedRequests()
        {
            try
            {
                ViewData["page"] = "listSignedRequests";
                ViewData["viewProcessAction"] = "";

                if (Session["uid"].ToString() == "")
                {
                    Response.Redirect("login");
                }

                string strUid = Session["uid"].ToString();
                return View(entityRquestObj.listSignedRequests(strUid, null, null, "", ""));
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "listSignedRequests:" + ex.Message);
                return View();  
            }

           
        }

        [HttpPost]
        public ActionResult listSignedRequests(DateTime? startDate, DateTime? endDate, string processAction, string applyReason)
        {
            try
            {
                ViewData["page"] = "listSignedRequests";
                ViewData["viewProcessAction"] = processAction;
                ViewData["viewApplyReason"] = applyReason;

                string strUid = sessionUID(startDate, endDate);
                return View(entityRquestObj.listSignedRequests(strUid, startDate, endDate, processAction, applyReason));
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "listSignedRequests post:" + ex.Message);
                return View();  
            }
        }
        
        public ActionResult Details(int id)
        {
            try
            {
                ViewBag.Message = "";
                ViewData["page"] = "listRequests";
                entityRequest entityRequestData = entityRquestObj.readApplyRequest(id);
                return View(entityRequestData);
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "Details :" + ex.Message);
                return View();
            }
        }

        public ActionResult bossSignView(int applyRequestID)
        {
            ViewData["applyRequestID"] = applyRequestID;
            return View();
        }

        [HttpPost]
        public ActionResult bossSignView(int applyRequestID, string rejectReasonText)
        {
            try
            {
                //submit 呼叫reject 函式
                if (!string.IsNullOrEmpty(rejectReasonText))
                {
                    reject(applyRequestID, rejectReasonText);
                    Response.Redirect("listWaitToSignRequests");
                }
                else
                {
                    Response.Redirect("bossSignView");
                }
                
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "bossSignView post :" + ex.Message);
                return View();
            }
        }

        public ActionResult rdDispatchSignView(int applyRequestID)
        {
            try
            {
                ViewData["applyRequestID"] = applyRequestID;
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdDispatchSignView :" + ex.Message);
                return View();
            }
        }
        
        [HttpPost]
        public ActionResult rdDispatchSignView(int applyRequestID, string taskUser)
        {
            try
            {
                if (!string.IsNullOrEmpty(taskUser))
                {
                    entityRequest entityApply = controlObj.readApplyRequest(applyRequestID);
                    controlObj.rdDispatch(entityApply.processType, entityApply.ikey, taskUser);
                }
                Response.Redirect("listWaitToSignRequests");
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdDispatchSignView post :" + ex.Message);
                return View();
            }
        }

        public ActionResult Sign(int intApplyRequestID, string strFlowName)
        {
            ViewBag.Message = "";
            Session["intApplyRequestID"] = intApplyRequestID;

            if (strFlowName.Trim() == "boss")
            {
                //主管
                Response.Redirect("bossSignView?applyRequestID="+intApplyRequestID);
            }
            else if (strFlowName.Trim() == "rdDispatch")
            {
                //RD 窗口
                Response.Redirect("rdDispatchSignView?applyRequestID=" + intApplyRequestID);
            }
            else if (strFlowName.Trim() == "rdAcceptTaskUser")
            {
                //RD 處理人員
                Response.Redirect("rdAcceptTaskUserSignView?applyRequestID=" + intApplyRequestID);
            }
            else if (strFlowName.Trim() == "complete")
            {
                Response.Redirect("rdCompleteSignView?applyRequestID=" + intApplyRequestID);
            }
            return View();
        }

        public ActionResult agree(int applyRequestID)
        {
            try
            {
                entityRequest entityApply = controlObj.readApplyRequest(applyRequestID);
                controlObj.agree(entityApply.processType, entityApply.ikey);
                Response.Redirect("listWaitToSignRequests");
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "agree :" + ex.Message);
                return View();
            }
        }

        private void reject(int applyRequestID,string rejectReason)
        {
            try
            {
                entityRequest entityApply = controlObj.readApplyRequest(applyRequestID);
                controlObj.reject(entityApply.processType, entityApply.ikey, rejectReason);
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "reject :" + ex.Message);
            }
        }

        /// <summary>
        /// 簽核的申請單內容
        /// </summary>
        /// <returns></returns>
        public ActionResult signViewTemplate()
        {
            try
            {
                int intApplyRequestID =(int) Session["intApplyRequestID"];
                entityRequest entityRequestData = new entityRequest();
                entityRequestData = controlObj.readApplyRequest(intApplyRequestID);
                return View(entityRequestData);
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "signViewTemplate :" + ex.Message);
                return View();
            }
        }

        public ActionResult rdAcceptTaskUserSignView(int applyRequestID)
        {
            try
            {
                ViewData["applyRequestID"] = applyRequestID;
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdAcceptTaskUserSignView :" + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult rdAcceptTaskUserSignView(int applyRequestID, string rejectReasonText, DateTime? predictCompleteDate,string submit)
        {
            try 
	        {
                if (submit == null)
                {
                    submit = "";
                }

                //submit 呼叫reject 函式
                if (submit.Trim()=="不同意")
                {
                    reject(applyRequestID, rejectReasonText);
                    Response.Redirect("listWaitToSignRequests");
                }
                else if (predictCompleteDate != null)
                {
                    entityRequest entityApply = controlObj.readApplyRequest(applyRequestID);
                    controlObj.agree(entityApply.processType, entityApply.ikey, predictCompleteDate);
                    Response.Redirect("listWaitToSignRequests");
                }
                else
                {
                    Response.Redirect("rdAcceptTaskUserSignView");
                }
                return View();
	        }
	        catch (Exception ex)
	        {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdAcceptTaskUserSignView :" + ex.Message);
                return View();
	        }
        }

        public ActionResult rdCompleteSignView(int applyRequestID)
        {
            try
            {
                ViewData["applyRequestID"] = applyRequestID;
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdCompleteSignView :" + ex.Message);
                return View();
            }
        }

        [HttpPost]
        public ActionResult rdCompleteSignView(int applyRequestID,bool complete)
        {
            try
            {
                if (complete == true)
                {
                    entityRequest entityApply = controlObj.readApplyRequest(applyRequestID);
                    controlObj.complete(entityApply.processType, entityApply.ikey);
                    Response.Redirect("listWaitToSignRequests");
                }
                
                return View();
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "rdCompleteSignView post:" + ex.Message);
                return View();
            }
        }

        public ActionResult edit(int id)
        {
            try
            {               
                 entityRequest entityRequestObj = entityRquestObj.readApplyRequest(id);
                 return View(entityRequestObj); 
            }
            catch (Exception ex)
            {
                  writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "edit:" + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult edit(int applyRequestID, DateTime applyDate, string applyTitle, string applyReason)
        {
            entityRequest entityRequestData = controlObj.readApplyRequest(applyRequestID);

            if (entityRequestData.processAction.Trim() == "boss" || (entityRequestData.processType == "flow2" && entityRequestData.processAction.Trim() == "rdDispatch"))
            {
                if (entityRequestData.applyUserID.ToString().Trim() == Session["uid"].ToString().Trim())
                {   

                    controlObj.edit(applyRequestID, applyDate, applyTitle, applyReason);
                }
                else
                {
                    ViewData["Message"] = "無法刪除(非)自已申請表單";
                }
            }
            else
            {
                ViewData["Message"] = "無法編輯已經進入下一流程的需求單";
            }

            string strUid = Session["uid"].ToString();
            return View("listRequests", entityRquestObj.listRequests(strUid, null, null, "", ""));
        }

        public ActionResult delete(int id)
        {
            try
            {
                entityRequest entityRequestData=  controlObj.readApplyRequest(id);
                ViewData["Message"] = null;
                ViewData["page"] = "listRequests";
                //主管尚未簽核許可之前，才能刪除
                if (entityRequestData.processAction.Trim() == "boss" || (entityRequestData.processType == "flow2" && entityRequestData.processAction.Trim() == "rdDispatch"))
                {
                    if (entityRequestData.applyUserID.ToString().Trim() == Session["uid"].ToString().Trim())
                    {
                        controlObj.delete(id);
                    }
                    else
                    {
                        ViewData["Message"] = "無法刪除(非)自已申請表單";
                    }
                }
                else
                {
                    ViewData["Message"] = "無法刪除已經進入下一流程的需求單";
                }

                string strUid = Session["uid"].ToString();
                return View("listRequests", entityRquestObj.listRequests(strUid,null,null,"",""));
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "delete:" + ex.Message);
                return View();
            }
        }

        public ActionResult applyAuthority()
        {
            IEnumerable<flowRole> liUsers= controlObj.listAllUsers();
            IEnumerable<SelectListItem> items =
            from value in liUsers
            select new SelectListItem
            {
                Text = value.strRoleUserName,
                Value = value.strRoleUserID
            };
            ViewBag.userID = items;
            ViewBag.userBossID = items;

            List<SelectListItem> powers = new List<SelectListItem>();
            powers.Add(new SelectListItem()
            {
                  Text="一般員工",
                  Value = "flow1"
            });

            powers.Add(new SelectListItem()
            {
                Text = "主管",
                Value = "flow2"
            });

            ViewBag.flow = powers;


            List<SelectListItem> departments = new List<SelectListItem>();
            departments.Add(new SelectListItem()
            {
                Text = "業務",
                Value = "1"
            });

            departments.Add(new SelectListItem()
            {
                Text = "工程",
                Value = "2"
            });

            departments.Add(new SelectListItem()
            {
                Text = "研發",
                Value = "3"
            });

            departments.Add(new SelectListItem()
            {
                Text = "客服",
                Value = "4"
            });

            departments.Add(new SelectListItem()
            {
                Text = "會計",
                Value = "5"
            });

            ViewBag.department = departments;
            return View(controlObj.listAuthorityRoles());
        }

        [HttpPost]
        public ActionResult applyAuthority(string userID, string userBossID, string gmail, string flow, string department)
        {
            try
            {

                controlObj.saveApplyAuthority(userID, userBossID, gmail, flow, department);

                IEnumerable<flowRole> liUsers = controlObj.listAllUsers();
                IEnumerable<SelectListItem> items =
                from value in liUsers
                select new SelectListItem
                {
                    Text = value.strRoleUserName,
                    Value = value.strRoleUserID
                };
                ViewBag.userID = items;
                ViewBag.userBossID = items;

                List<SelectListItem> powers = new List<SelectListItem>();
                powers.Add(new SelectListItem()
                {
                    Text = "一般員工",
                    Value = "flow1"
                });

                powers.Add(new SelectListItem()
                {
                    Text = "主管",
                    Value = "flow2"
                });

                ViewBag.flow = powers;


                List<SelectListItem> departments = new List<SelectListItem>();
                departments.Add(new SelectListItem()
                {
                    Text = "業務",
                    Value = "1"
                });

                departments.Add(new SelectListItem()
                {
                    Text = "工程",
                    Value = "2"
                });

                departments.Add(new SelectListItem()
                {
                    Text = "研發",
                    Value = "3"
                });

                departments.Add(new SelectListItem()
                {
                    Text = "客服",
                    Value = "4"
                });


                departments.Add(new SelectListItem()
                {
                    Text = "會計",
                    Value = "5"
                });

                ViewBag.department = departments;
                return View(controlObj.listAuthorityRoles());
            }
            catch (Exception ex)
            {
                writeObj.writeToFile("applyResut_error" + DateTime.Today.ToString("yyyyMMdd"), path, "applyAuthority post:" + ex.Message);
                return View();
            }
            
        }
    }
}