using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OLX.DA.Admin;
using OLX.DA.User;
using OLX.Models.User;

namespace OLXMVCApp.Controllers.Users
{
    public class ChatController : Controller
    {
        // GET: Chat
     

        public ActionResult select()
        {
            Chat c = new Chat();
            
            ChatMddual model = new ChatMddual();
            model.model = c.GetChatMdduals();
            //chatMddual.ChatList = new List<SelectListItem>();

            return View(model);
            //List<ChatMddual> obj = db.GetChatMdduals();
            //List<SelectListItem> selectList = new List<SelectListItem>();
            //return View(obj);

            //DBContext dbuser = new DBContext();
            //List<users> objuser = db.GetUsersmodual();
            //return View(objuser);

        }
        [HttpPost]
        public ActionResult select(ChatMddual chat)
        {

            if (ModelState.IsValid == true)
            {
                Chat DBContext = new Chat();

              
                string chatMessage = Request.Form["msg"];
                bool chack = DBContext.InsertChat(chatMessage);
                //chat.Chat1 = Request.Form["msg"];
                if (chack == true)
                {
                    TempData["InsertMsg"] = "Data Inserted";
                    ModelState.Clear();
                    return RedirectToAction("select", "Chat");
                }
            }
            Chat dbContext = new Chat();
            List<ChatMddual> chatList = dbContext.GetChatMdduals();
            return View(chatList);
        }
    }
}