using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_TagPost : BaseRepository<TagPost>
    {
        private DatabaseEntities _context;
        public Entity_TagPost(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public List<TagPost> GetAllByPostId(int postId)
        {
            return _context.TagPost.Where(p =>
                p.PostId == postId
            ).ToList();
        }

        public void DeleteByPostId(int postId)
        {
            List<TagPost> list = GetAllByPostId(postId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }
}
