using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_GalleryLanguage : BaseRepository<GalleryLanguage>
    {

        private DatabaseEntities _context;
        public Entity_GalleryLanguage(DatabaseEntities context) : base(context)
        {
            _context = context;
        }

        public List<GalleryLanguage> GetAllByGalleryId(int galleryId)
        {
            return _context.GalleryLanguage.Where(g =>
                g.GalleryId == galleryId
            ).ToList();
        }

        public void DeleteByGalleryId(int galleryId)
        {
            List<GalleryLanguage> list = GetAllByGalleryId(galleryId);
            for (int i = 0; i < list.Count; i++)
            {
                Delete(list[i]);
            }
        }
    }




}
