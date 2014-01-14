﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using ApiCore.AttachmentTypes;

namespace ApiCore.Wall
{
    public class WallFactory : BaseFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public int PostsCount = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public WallFactory(ApiManager manager)
            : base(manager)
        {
            this.Manager = manager;
        }

        private Attachment getAttachment(XmlNode fromPost)
        {

            if (fromPost != null)
            {
                Attachment attachment = new Attachment();
                string attachmentType = fromPost.SelectSingleNode("type").InnerText;
                switch (attachmentType)
                {
                    case "app":
                    {
                        attachment.Type = AttachmentType.Application;
                        break;
                    }
                    case "graffiti":
                    {
                        attachment.Type = AttachmentType.Graffiti;
                        break;
                    }
                    case "video":
                    {
                        attachment.Type = AttachmentType.Video;
                        break;
                    }
                    case "audio":
                    {
                        attachment.Type = AttachmentType.Audio;
                        break;
                    }
                    case "doc":
                    {
                        attachment.Type = AttachmentType.Document;
                        break;
                    }
                    case "photo":
                    {
                        attachment.Type = AttachmentType.Photo;
                        break;
                    }
                    case "posted_photo":
                    {
                        attachment.Type = AttachmentType.PostedPhoto;
                        break;
                    }
                    case "note":
                    {
                        attachment.Type = AttachmentType.Note;
                        break;
                    }
                    case "poll":
                    {
                        attachment.Type = AttachmentType.Poll;
                        break;
                    }
                    case "link":
                    {
                        attachment.Type = AttachmentType.Url;
                        break;
                    }
                    case "checkin":
                    {
                        attachment.Type = AttachmentType.Checkin;
                        break;
                    }
                    case "share":
                    {
                        attachment.Type = AttachmentType.Share;
                        break;
                    }
                }
                XmlNode attachmentData = fromPost.SelectSingleNode(attachmentType);
                if (attachmentData != null)
                {
                    attachment.Data = AttachmentFactory.GetAttachment(attachment.Type, attachmentData);
                }
                else
                {
                    attachment.Data = null;
                }
                //attachment.ItemId = Convert.ToInt32(fromPost.SelectSingleNode("item_id").InnerText);
                //if (attachment.Type != AttachmentType.Graffiti)
                //{
                //    attachment.OwnerId = Convert.ToInt32(fromPost.SelectSingleNode("owner_id").InnerText);
                //}
                //if (attachment.Type == AttachmentType.Application)
                //{
                //    attachment.ApplicationId = Convert.ToInt32(fromPost.SelectSingleNode("app_id").InnerText);
                //}
                //if (fromPost.SelectSingleNode("thumb_src") != null)
                //{
                //    attachment.ThumbnailUrl = fromPost.SelectSingleNode("thumb_src").InnerText;
                //}
                return attachment;
            }
            return null;
        }

        private AttachmentData getAttachmentData(AttachmentType type, XmlNode attachmentData)
        {
            return AttachmentFactory.GetAttachment(type, attachmentData);
        }

        private List<WallEntry> buildEntryList(XmlDocument x)
        {
            XmlNodeList msgsNodes = x.SelectNodes("/response/post");
            if (msgsNodes != null)
            {
                List<WallEntry> msgList = new List<WallEntry>();
                foreach (XmlNode msgNode in msgsNodes)
                {
                    XmlUtils.UseNode(msgNode);
                    WallEntry wall = new WallEntry();
                    wall.Id = XmlUtils.Int("id");
                    wall.Body = XmlUtils.String("text");
                    wall.FromUser = XmlUtils.Int("from_id");
                    wall.ToUser = XmlUtils.Int("to_id");
                    wall.Date = CommonUtils.FromUnixTime(XmlUtils.String("date"));
                    wall.Online = ((XmlUtils.String("online")) == "1" ? true : false);
                    wall.Attachment = this.getAttachment(msgNode.SelectSingleNode("attachment"));
                    wall.CopyOwnerId = XmlUtils.Int("copy_owner_id");
                    wall.CopyPostId = XmlUtils.Int("copy_post_id");
                    wall.LikesInfo = LikesFactory.GetLikesInfo(msgNode.SelectSingleNode("likes"));
                    wall.CommentsInfo = CommentsFactory.GetCommentsInfo(msgNode.SelectSingleNode("comments"));
                    if (XmlUtils.Int("reply_count") != -1)
                    {
                        wall.RepliesCount = XmlUtils.Int("reply_count");
                    }
                    msgList.Add(wall);
                }
                return msgList;
            }
            return null;
        }

        /// <summary>
        /// Get wall entries
        /// </summary>
        /// <param name="ownerId">if null - wall entries for current user will be returned, else - for specified user</param>
        /// <param name="count">messages count. null allowed</param>
        /// <param name="offset">message offset. null allowed</param>
        /// <returns>Wall entries list or null</returns>
        public List<WallEntry> Get(int? ownerId, int? count, int? offset, string filter)
        {
            this.Manager.Method("wall.get", new object[] { "owner_id", ownerId, "offset", offset, "count", count, "filter", filter });
            XmlDocument x = this.Manager.Execute().GetResponseXml();
            if (x.InnerText.Equals(""))
            {
                this.Manager.DebugMessage(string.Format("No wall entries found with specified args"));
                return null;
            }
            this.PostsCount = Convert.ToInt32(x.SelectSingleNode("/response/count").InnerText);

            return this.buildEntryList(x);
        }

        /// <summary>
        /// Post message in the wall
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be posted, else - for specified user</param>
        /// <param name="message">message to be posted</param>
        /// <returns>message id</returns>
        public int Post(int? ownerId, string message)
        {
            this.Manager.Method("wall.post", new object[] { "owner_id", ownerId, "message", message });
            XmlDocument x = this.Manager.Execute().GetResponseXml();

            return Convert.ToInt32(x.InnerText);
        }

        /// <summary>
        /// Post message in the wall
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be posted, else - for specified user</param>
        /// <param name="message">message to be posted</param>
        /// <param name="attachment">attachment to be added to wall post</param>
        /// <param name="services">services wich this post be reposted</param>
        /// <returns>message id</returns>
        public int Post(int? ownerId, string message, MessageAttachment attachment, string[] services)
        {
            return this.Post(ownerId, message, new MessageAttachment[]{ attachment}, services);
        }


        public int Post(int? ownerId, string message, MessageAttachment[] attachments, string[] services)
        {
            this.Manager.Method("wall.post", new object[] { "owner_id", ownerId, 
                                    "message", message, 
                                    "attachment", attachments, 
                                    "services", ((services != null)?String.Join(",", services):null) });
            XmlDocument x = this.Manager.Execute().GetResponseXml();

            return Convert.ToInt32(x.InnerText);
        }

        public int Post(int? ownerId, string message, MessageAttachment[] attachments)
        {
            return this.Post(ownerId, message, attachments, null);
        }

        /// <summary>
        /// Delete wall post
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be deleted, else - for specified user</param>
        /// <param name="msg_id">post id to delete</param>
        /// <returns>true if all ok, else false</returns>
        public bool Delete(int? ownerId, int msg_id)
        {
            this.Manager.Method("wall.delete", new object[] { "owner_id", ownerId, "mid", msg_id });
            XmlDocument x = this.Manager.Execute().GetResponseXml();

            return ((x.SelectSingleNode("/response").InnerText.Equals("1")) ? true : false);
        }

        /// <summary>
        /// Restore deleted entry
        /// </summary>
        /// <param name="ownerId">if null - wall entry for current user will be restored, else - for specified user</param>
        /// <param name="msg_id">post id to restore</param>
        /// <returns>true if all ok, else false</returns>
        public bool Restore(int? ownerId, int msg_id)
        {
            this.Manager.Method("wall.restore", new object[] { "owner_id", ownerId, "mid", msg_id });
            XmlDocument x = this.Manager.Execute().GetResponseXml();
            return ((x.InnerText.Equals("1")) ? true : false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="needPublish"></param>
        /// <returns></returns>
        public int AddLike(int postId, bool needPublish)
        {
            this.Manager.Method("wall.addLike", new object[] { "post_id", postId, "need_publish", needPublish });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml().SelectSingleNode("/response"));

            return XmlUtils.Int("likes");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public int DeleteLike(int postId)
        {
            this.Manager.Method("wall.deleteLike", new object[] { "post_id", postId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml().SelectSingleNode("/response"));

            return XmlUtils.Int("likes");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="postId"></param>
        /// <param name="sortOrder"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<EntryComment> GetComments(int? ownerId, int postId, SortOrder sortOrder, int? offset, int? count)
        {
            this.Manager.Method("wall.getComments", new object[] { "owner_id", ownerId, "post_id", postId, "sort", ((sortOrder == SortOrder.Asc) ? "asc" : "desc"), "offset", offset, "count", count });
            XmlNodeList nodes = this.Manager.Execute().GetResponseXml().SelectNodes("/response/comment");
            if (nodes.Count > 0)
            {
                List<EntryComment> comments = new List<EntryComment>();
                foreach (XmlNode node in nodes)
                {
                    EntryComment c = CommentsFactory.GetEntryComment(node);
                    comments.Add(c);
                }

                return comments;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="postId"></param>
        /// <param name="msg"></param>
        /// <param name="replyToCid"></param>
        /// <returns></returns>
        public int AddComment(int? ownerId, int postId, string msg, int? replyToCid)
        {
            this.Manager.Method("wall.addComment", new object[] { "owner_id", ownerId, "post_id", postId, "text", msg, "reply_to_cid", replyToCid });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml().SelectSingleNode("/response"));

            return XmlUtils.Int("cid");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool DeleteComment(int? ownerId, int commentId)
        {
            this.Manager.Method("wall.deleteComment", new object[] { "owner_id", ownerId, "cid", commentId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.Bool("response");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public bool RestoreComment(int? ownerId, int commentId)
        {
            this.Manager.Method("wall.restoreComment", new object[] { "owner_id", ownerId, "cid", commentId });
            XmlUtils.UseNode(this.Manager.Execute().GetResponseXml());

            return XmlUtils.Bool("response");
        }



        public Photos.PhotoEntryFull UploadPhotoToMyWall(string pathToPhoto)
        {
            Photos.PhotosFactory pf = new Photos.PhotosFactory(this.Manager);
            string url = pf.GetWallUploadServer(null, null);
            HttpUploaderFactory uf = new HttpUploaderFactory();
            NameValueCollection files = new NameValueCollection();
            files.Add("photo", pathToPhoto);
            ApiCore.Photos.PhotoUploadedInfo ui = new ApiCore.Photos.PhotoUploadedInfo(uf.Upload(url, null, files));
            return pf.SaveWallPhoto(ui, null, null);
        }
    }
}
