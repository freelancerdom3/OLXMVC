using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OLX.DA.Admin;
using OLX.DA.User;


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
                    return View("select");
                }
            }
            Chat dbContext = new Chat();
            List<ChatMddual> chatList = dbContext.GetChatMdduals();
            return View(chatList);
        }

        public ActionResult chatmap( int userid, int advertiseid)
        {

            ChatMappingModel mappingModel = new ChatMappingModel()
            {
                Sellerid=userid,
                advertiseid=advertiseid
            };

           Chat chat = new Chat();

            chat.InsertInMap(mappingModel);

            TempData["mapid"] = advertiseid;
            return View("select");
        }

        [HttpPost]
        public ActionResult chatmap()
        {

            string Message = Request.Form["msg"];
            Chat chat = new Chat();
            ChatsModel chatsModel = new ChatsModel();
            if (Session["userid"]!=null) 
            {

                int advertiseid = (int)TempData["mapid"];
                int mapid = chat.getMapid(advertiseid);
                bool check=chat.EnterMessage(mapid, Message);
                if (check)
                {
                    return View("chatmap");
                }

            }
          
            return View();
                     

        }
    }
}