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

        Chat db = new Chat();
        public ActionResult select()
        {
            Chat c = new Chat();
            
           // ChatMddual model = new ChatMddual();
            ChatsModel chats = new ChatsModel();
            // List<ChatsModel>model=c.GetChatModel();
            chats.model = c.GetChatModel();
           // model.model = c.GetChatMdduals();
            //chatMddual.ChatList = new List<SelectListItem>();

            return View(chats);
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

        public ActionResult seller(int userid, int advertiseid)
        {
            ChatMappingModel chatMappingModel = new ChatMappingModel()
            {
                Sellerid = userid,
                advertiseid=advertiseid
            };
            Chat chat = new Chat();
            chat.InsertInMap(chatMappingModel);
            return View("chatmap");
        }

        public ActionResult chatmap( int userid, int advertiseid)
        {
            Chat chat = new Chat();
            int loginid = (int)Session["userid"];

            int ifseller =chat.isseller(advertiseid);
            if (ifseller == loginid)
            {
               

                ChatMappingModel map = new ChatMappingModel()
                {
                    Sellerid = loginid
                };

                
                    
            }
             
            
            ChatMappingModel mappingModel = new ChatMappingModel()
            {
                Buyerid=userid,
                advertiseid=advertiseid
            };

           

            chat.InsertInMap(mappingModel);

            TempData["adid"] = advertiseid;

            TempData["sellid"] = userid;
            List<ChatMappingModel> chats = chat.show();


            return View("select");
        }

        [HttpPost]
        public ActionResult chatmap()
        {
            bool buyorsell=true;
            string Message = Request.Form["msg"];
            Chat chat = new Chat();
            ChatsModel chatsModel = new ChatsModel();
            if (Session["userid"]!=null)
            {
                int buyid = (int)Session["userid"];
                int sellid= (int)TempData.Peek("sellid");
                if (buyid==sellid)
                {
                     buyorsell = false;
                }
                 
                    int advertiseid = (int)TempData.Peek("adid");
                int mapid = chat.getMapid(advertiseid, buyid, sellid);
               
                bool check = chat.EnterMessage(mapid, Message,buyorsell);
                    if (check)
                    {
                        return View("select");
                    }

            }
          
            return View();
        }
        public ActionResult getname(int adid)
        {
            //int advid= (int)TempData.Peek("adid");

            List<GetnameModel> names = db.getName(adid);
            return View(names);

        }
        public ActionResult chatButton()
        {
            int userid = (int)Session["userid"];
            Chat chat = new Chat();
            int adid = chat.getadvertiseid(userid);

            List<GetnameModel> names = db.getName(adid);
            return View(names);

          
         

        }
    }
}