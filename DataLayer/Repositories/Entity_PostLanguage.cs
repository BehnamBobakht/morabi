using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_PostLanguage : BaseRepository<PostLanguage>
    {

        private DatabaseEntities _context;
        public Entity_PostLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<PostLanguage> GetAllByPostId(int postId)
        {
            return _context.PostLanguage.Where(p =>
                p.PostId == postId
            ).ToList();
        }

        public void DeleteByPostId(int postId)
        {
            List<PostLanguage> list = GetAllByPostId(postId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
