using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class entityProcessFlow
    {
        private List<FlowItem> flow1;
        private List<FlowItem> flow2;

        public entityProcessFlow()
          {
              setFlow1(out flow1);
              setFlow2(out flow2);
          }
        
        private void setFlow1(out List<FlowItem> flow)
        {
            try
            {
                
                List<FlowItem> flowSample = new List<FlowItem>();

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 0,
                    flowItemName = "申請人",
                    flowRoleFunc = "apply",
                    flowItemView = "applyRequest",                   
                    flowItemNextID = 1
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 1,
                    flowItemName = "主管",
                    flowRoleFunc = "boss",
                    flowItemView = "bossSignView",
                    flowActions = new List<flowAction>(){
                        new flowAction()
                        {
                            title = "核可",
                            value = "agree"
                        },
                        new flowAction(){
                             title = "退回",
                            value = "reject"
                        },
                    },
                    flowItemNextID = 2
                });

           
                flowSample.Add(new FlowItem()
                {
                    flowItemID = 2,
                    flowItemName = "RD窗口",
                    flowRoleFunc = "rdDispatch",
                    flowItemView = "rdDispatchSignView",
                    flowActions = null,
                    flowItemNextID = 3
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 3,
                    flowItemName = "RD處理人員",
                    flowRoleFunc = "rdAcceptTaskUser",
                    flowItemView = "rdDispatchSignView",
                    flowActions = new List<flowAction>(){
                        new flowAction()
                        {
                            title = "同意",
                            value = "agree"
                        },
                        new flowAction(){
                             title = "退回",
                            value = "reject"
                        },
                    },
                    flowItemNextID = 4
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 4,
                    flowItemName = "完工確認",
                    flowRoleFunc = "complete",
                    flowItemView = "rdCompleteSignView",
                    flowActions = new List<flowAction>()
                    {
                         new flowAction(){
                              title="完工",
                              value="complete"
                         }
                    },
                    flowItemNextID = 0//結束
                });


                flow = flowSample;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void setFlow2(out List<FlowItem> flow)
        {
            try
            {

                List<FlowItem> flowSample = new List<FlowItem>();

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 0,
                    flowItemName = "申請人",
                    flowRoleFunc = "apply",
                    flowItemView = "applyRequest",
                    flowItemNextID = 1
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 1,
                    flowItemName = "RD窗口",
                    flowRoleFunc = "rdDispatch",
                    flowItemView = "rdDispatchSignView",
                    flowActions = null,
                    flowItemNextID = 2
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 2,
                    flowItemName = "RD處理人員",
                    flowRoleFunc = "rdAcceptTaskUser",
                    flowItemView = "rdDispatchSignView",
                    flowActions = new List<flowAction>(){
                        new flowAction()
                        {
                            title = "同意",
                            value = "accept"
                        },
                        new flowAction(){
                             title = "退回",
                            value = "reject"
                        },
                    },
                    flowItemNextID = 3
                });

                flowSample.Add(new FlowItem()
                {
                    flowItemID = 3,
                    flowItemName = "完工確認",
                    flowRoleFunc = "complete",
                    flowItemView = "rdCompleteSignView",
                    flowActions = new List<flowAction>()
                    {
                         new flowAction(){
                              title="完工",
                              value="complete"
                         }
                    },
                    flowItemNextID = 0//結束
                });

                flow = flowSample;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private FlowItem getFlow1Next(int flowItemID)
        {
            try
            {
                int intFlowNextID = flow1.Where(m => m.flowItemID == flowItemID).FirstOrDefault().flowItemNextID;

                FlowItem entityNextItemObj = flow1.Where(n => n.flowItemID ==intFlowNextID).FirstOrDefault();

                return entityNextItemObj;
            }
            catch (Exception ex)
            {   
                throw new Exception(ex.Message);
            }
        }

        private FlowItem getFlow2Next(int flowItemID)
        {
            try
            {
                int intFlowNextID = flow2.Where(m => m.flowItemID == flowItemID).FirstOrDefault().flowItemNextID;

                FlowItem entityNextItemObj = flow2.Where(n => n.flowItemID == intFlowNextID).FirstOrDefault();

                return entityNextItemObj;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public FlowItem  getFlowNext(string strFlowType,int intFlowItemID)
        {
            if (strFlowType == "flow1")
            {
                return getFlow1Next(intFlowItemID);
            }
            else if (strFlowType == "flow2")
            {
                return getFlow2Next(intFlowItemID);
            }
            else
            {
                return getFlow1Next(intFlowItemID);
            }

        }

        public FlowItem getFlowCurrentItem(string strFlowType, int flowItemID)
        {
            try
            {
                switch (strFlowType)
                {
                    case "flow1":
                        return flow1.Where(m => m.flowItemID == flowItemID).FirstOrDefault();

                    case "flow2":
                        return flow2.Where(m => m.flowItemID == flowItemID).FirstOrDefault();

                    default:
                        return flow1.Where(m => m.flowItemID == flowItemID).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}