using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_ShopResellerStory : BaseRepository<ShopResellerStory>
    {
        private DatabaseEntities _context;
        public Entity_ShopResellerStory(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public void InsertNewStory(Enum_StoryType storyType, int? resellerId, int? targetId, int? pictureId, string description)
        {
            ShopResellerStory story = new ShopResellerStory
            {
                CreateDatetime = DateTime.Now,
                ShopResellerId = (int)resellerId,
                TypeId = (byte)storyType,
                PictureId = pictureId,
                Description = description
            };
            switch (storyType)
            {
                case Enum_StoryType.COLLECTION:
                    story.CollectionId = targetId;
                    break;
                case Enum_StoryType.GALLERY:
                    story.GalleryId = targetId;
                    break;
                case Enum_StoryType.PRODUCT:
                    story.ProductId = targetId;
                    break;
                default:
                    break;
            }
            Insert(story);
            Save();
        }

        public List<ShopResellerStory> Search(DateTime? dateTime = null)
        {
            dateTime = dateTime == null ? DateTime.Now.AddMonths(-1) : dateTime;
            return _context.ShopResellerStory.Where(p => p.CreateDatetime >= dateTime).ToList();
        }
    }
}
