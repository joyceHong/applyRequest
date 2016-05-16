using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using applyRequests;
using applyRequests.Controllers;
using applyRequests.Models;

namespace applyRequests.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        controlDoActioncs controlDoActionObj = new controlDoActioncs();
        controlEmail controlSendMail = new controlEmail();


        [TestMethod]
        public void controlProcessFlow_GetNextFlowIndex()
        {
            //entityProcessFlow controlProcessFlowObj = new entityProcessFlow();
            //FlowItem entityFlowItemObj = controlProcessFlowObj.getFlowNext("flow1",2);
        }

        [TestMethod]
        public void controlDoAction_bosRole()
        {
            flowRole flowRoleObj = controlDoActionObj.bossRole("0137");
        }

        [TestMethod]
        public void entityFlowRole_rdDispatch()
        {
            flowRole flowRoleObj = controlDoActionObj.rdDispatchRole();
        }

        [TestMethod]
        public void entityFlowRole_listRdTaskUsers()
        {
            IEnumerable<flowRole> listRdTaskUsers = controlDoActionObj.listRdAcceptTaskUsers();
        }

        [TestMethod]
        public void entityFlowRole_rdTaskUser()
        {
            flowRole flowRoleObj = controlDoActionObj.rdAcceptTaskUser(2);
        }

        [TestMethod]
        public void control_sendMail()
        {

            entityRequest entityRequest = controlDoActionObj.readApplyRequest(2);
            flowRole flowRoleUser = new flowRole();
            flowRoleUser.strEmail = "joyceHong0827@gmail.com";
            controlSendMail.sendMail(entityRequest, flowRoleUser);
        }

        [TestMethod]
        public void sendMail()
        {
            controlDoActionObj.sendMail(43);
        }


         [TestMethod]
        public void readUserFlowRole()
        {
            entityFlowRole obj = new entityFlowRole();
            flowRole test = obj.readUserFlowRole("0019");
        }

        //public flowRole readUserFlowRole(string strApplyUserID)

    }
}
