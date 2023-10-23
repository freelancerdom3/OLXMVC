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
            showChatModel showChat = new showChatModel();
            int map = (int)TempData.Peek("mapid");
            showChat.model = c.showchat(map);
            chats.model = c.GetChatModel();
           // model.model = c.GetChatMdduals();
            //chatMddual.ChatList = new List<SelectListItem>();

            return View(showChat);
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

        public ActionResult seller()
        {

            ChatMappingModel chatMappingModel = new ChatMappingModel()
            {
                Buyerid = (int)TempData["buyid"],
                advertiseid= (int)TempData["adid"],
                Sellerid=(int)Session["userid"]
            };
            TempData["buyid"] = chatMappingModel.Buyerid;
            TempData["adid"] = chatMappingModel.advertiseid;
            Chat chat = new Chat();
            chat.InsertInMap(chatMappingModel);
            return View("select");
        }

        public ActionResult chatmap( int userid, int advertiseid,int? mapid)
        {
            Chat chat = new Chat();
            int loginid = (int)Session["userid"];
            TempData["adid"] = advertiseid;
            TempData["buyid"] = userid;
            int ifseller =chat.isseller(advertiseid);
            ChatMappingModel mappingModel = null;
            if (ifseller == loginid)
            {
                TempData["session"] = userid;
                TempData["user"] = ifseller;
                mappingModel = new ChatMappingModel()
                {
                    Buyerid=userid,
                    Sellerid=(int)Session["userid"],
                    advertiseid=advertiseid
                    
                };
              
             
                //return RedirectToAction("seller","chat");
   
            }
            else 
            {
                TempData["session"] = (int)Session["userid"];
                TempData["user"] = userid;
                mappingModel = new ChatMappingModel()
               {
                Buyerid=(int)Session["userid"],
                Sellerid=userid,
                advertiseid=advertiseid,
                
              };
                mapid = chat.getMapid(advertiseid,loginid,userid);
            }
            chat.InsertInMap(mappingModel);

            
            

            TempData["adid"] = advertiseid;

           // int? mappingid = (int)mapid;
            //TempData["sellid"] = userid;
            //List<ChatMappingModel> chats = chat.show();
            if (mapid>0)
            {

                TempData["mapid"] = mapid;
                return RedirectToAction("select","chat");

            }
            return View("select");
        }

        [HttpPost]
        public ActionResult chatmap()
        {

            bool buyorsell=true;
            string Message = Request.Form["msg"];
            if (Message=="".Trim())
            {
                return RedirectToAction("select", "chat");
            }
            Chat chat = new Chat();
            ChatsModel chatsModel = new ChatsModel();
            ChatMappingModel map = new ChatMappingModel();
           
                 int session = (int)TempData.Peek("session");
            int user = (int)TempData.Peek("user");
                int sell = (int)Session["userid"];
                if (sell==user)
                {
                   buyorsell = false;
                }
                 
                    int advertiseid = (int)TempData["adid"];
                int mapid = chat.getMapid(advertiseid, session, user);
            TempData["mapid"] = mapid;
            bool check = chat.EnterMessage(mapid, Message,buyorsell);
            
                    if (check)
                    {
                    
                //List<showChatModel> model = chat.showchat(mapid);
                return RedirectToAction("select", "chat");
                    //return View("select");
                    }

            
          
            return View();
        }
      public ActionResult showchat(int mapid)
        { 
            Chat chat = new Chat();
            List<showChatModel> model = chat.showchat(mapid);
            return View(model);
        }
        public ActionResult getname(int? adid)
        {
            //int advid= (int)TempData.Peek("adid");

            List<GetnameModel> names = db.getName(adid);
            return View(names);

        }
        public ActionResult chatButton()
        {
            int userid = (int)Session["userid"];
            Chat chat = new Chat();
            //int adid = chat.getadvertiseid(userid);
            List<int> adIds = chat.getAdvertiseIds(userid);
            List<GetnameModel> names = new List<GetnameModel>();
            foreach (int adid in adIds)
            {
                List<GetnameModel> adnames = db.getName(adid);
                names.AddRange(adnames);
            }
          
            return View(names);

          
         

        }
    }
}