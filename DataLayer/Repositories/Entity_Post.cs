using Binbin.Linq;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using ExpressMapper.Extensions;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class Entity_Post : BaseRepository<Post>
    {
        DatabaseEntities _context;
        public Entity_Post(DatabaseEntities context) : base(context)
        {
            _context = context;
        }
        public PostVm GetPostVm(int id, bool visit = false)
        {
            var post = GetById(id);
            if (post != null && post.Active != false)
            {
                if (visit)
                {
                    post.VisitCount++;
                    Save();
                }

                return post.Map<Post, PostVm>();
            }
            return null;
        }
        public List<Post> Search(int? websiteId = null, int? notId = null, int? categoryId = null, string categoryLabel = null, int? typeId = null, int index = 1, int pageSize = 10, string keyword = null, string name = null, Enum_PostOrder order = Enum_PostOrder.NONE, Enum_Lang lang = Enum_Lang.NONE, string url = null, int? stateId = null)
        {
            var MyQuery = GetSearchQuery(websiteId: websiteId, notId: notId, categoryId: categoryId, categoryLabel: categoryLabel, typeId: typeId, keyword: keyword, name: name, lang: lang, url: url, stateId: stateId);
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;

            if (order == Enum_PostOrder.NEW)
                return _context.Post.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.OLD)
                return _context.Post.OrderBy(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.MORE_VISIT)
                return _context.Post.OrderByDescending(p => p.VisitCount).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.NAME)
                return _context.Post.OrderBy(p => p.Name).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.SHOWNUMBER)
                return _context.Post.Where(MyQuery).OrderBy(p => p.ShowNumber.HasValue ? 0 : 1).ThenBy(p => p.ShowNumber).ThenByDescending(p => p.ID).Skip(skipValue).Take(pageValue).ToList();
            else
                return _context.Post.OrderByDescending(p => p.ID).Where(MyQuery).Skip(skipValue).Take(pageValue).ToList();
        }
        public PostSearchVm SearchVm(int? notId = null, int? categoryId = null, string categoryLabel = null, string categoryLabel2 = null , int? typeId = null, int index = 1, int pageSize = 10, string keyword = null, string name = null, Enum_PostOrder order = Enum_PostOrder.NONE,bool? active=true)
        {
            var result = new PostSearchVm();
            var posts = new List<Post>();
            var MyQuery = GetSearchQuery2(
                notId: notId, categoryId: categoryId, categoryLabel: categoryLabel,categoryLabel2:categoryLabel2, typeId: typeId, keyword: keyword, name: name,active:active
                );
            int skipValue = pageSize * (index - 1);
            int pageValue = pageSize;
            IQueryable<Post> q = _context.Post.AsNoTracking().Where(MyQuery);

            if (order == Enum_PostOrder.NEW)
                posts= q.OrderByDescending(p => p.ID).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.OLD)
                posts= q.OrderBy(p => p.ID).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.MORE_VISIT)
                posts = q.OrderByDescending(p => p.VisitCount).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.NAME)
                posts = q.OrderBy(p => p.Name).Skip(skipValue).Take(pageValue).ToList();
            else if (order == Enum_PostOrder.SHOWNUMBER)
                posts = q.OrderBy(p => p.ShowNumber.HasValue ? 0 : 1).ThenBy(p => p.ShowNumber).ThenByDescending(p => p.ID).Skip(skipValue).Take(pageValue).ToList();
            else
                posts = q.OrderBy(p => p.ShowNumber.HasValue ? 0 : 1).ThenBy(p => p.ShowNumber).ThenByDescending(s => s.ID).Skip(skipValue).Take(pageValue).ToList();
            result.Posts = posts.Map<List<Post>,List<PostVm>>();
            result.TotalCount = q.Count();
            return result;
        }

        public int SearchCount(int? websiteId = null, int? notId = null, int? categoryId = null, string categoryLabel = null, string keyword = null, string name = null, Enum_Lang lang = Enum_Lang.NONE, int? stateId = null)
        {
            var MyQuery = GetSearchQuery(websiteId: websiteId, notId: notId, categoryId: categoryId, categoryLabel: categoryLabel, keyword: keyword, name: name, stateId: stateId);
            return _context.Post.Count(MyQuery);
        }

        private Expression<Func<Post, bool>> GetSearchQuery(int? websiteId = null, int? notId = null, int? categoryId = null, string categoryLabel = null, int? typeId = null, string keyword = null, string name = null, Enum_Lang lang = Enum_Lang.NONE, string url = null, int? stateId = null)
        {
            var MyQuery = PredicateBuilder.True<Post>();
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
            if (websiteId != null && websiteId != 0)
                MyQuery = MyQuery.And(p => p.WebsiteId == websiteId);
            if (categoryId != null && categoryId != 0)
                MyQuery = MyQuery.And(p => p.CategoryId == categoryId);
            if (categoryLabel != null)
                MyQuery = MyQuery.And(p => p.Category.Label == categoryLabel);
            if (typeId != null && typeId != 0)
                MyQuery = MyQuery.And(p => p.Category.TypeId == typeId);
            if (keyword != null)
                MyQuery = MyQuery.And(p => p.Keywords.Contains(keyword));
            if (url != null)
                MyQuery = MyQuery.And(p => p.Category.URL.Contains(url));
            if (stateId != null)
                MyQuery = MyQuery.And(p => p.StateId == stateId);
            if (IsNullOrEmptyId(name) == false)
            {
                name = name.ToPersianChar();
                var nameQuery = PredicateBuilder.True<Post>();

                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) ||
                    p.PostLanguage.Any(ss => ss.Name.Contains(item) || (ss.Keywords.Contains(item)) || (ss.Summary.Contains(item))) ||
                    (p.Keywords.Contains(item)) ||
                    (p.Summary.Contains(item)));
                }
                MyQuery = MyQuery.And(nameQuery);
            }
            return MyQuery;
        }
        private Expression<Func<Post, bool>> GetSearchQuery2(int? notId = null, int? categoryId = null, string categoryLabel = null, string categoryLabel2 = null, int? typeId = null, string keyword = null, string name = null,bool? active=true)
        {
            var MyQuery = PredicateBuilder.True<Post>();
            if (active != null)
                MyQuery = MyQuery.And(p => p.Active==active); 
            if (notId != null && notId != 0)
                MyQuery = MyQuery.And(p => p.ID != notId);
               
            if (categoryId != null && categoryId != 0)
                MyQuery = MyQuery.And(p => p.CategoryId == categoryId);
            if (!string.IsNullOrEmpty( categoryLabel))
                MyQuery = MyQuery.And(p => p.Category.Label.ToLower() == categoryLabel.ToLower());
            if (!string.IsNullOrEmpty(categoryLabel2))
                MyQuery = MyQuery.And(p => p.Category.Label.Contains(categoryLabel2));

            if (typeId != null && typeId != 0)
                MyQuery = MyQuery.And(p => p.Category.TypeId == typeId);
            if (keyword != null)
                MyQuery = MyQuery.And(p => p.Keywords.Contains(keyword));
               
            if (IsNullOrEmptyId(name) == false)
            {
                name = name.ToPersianChar();
                var nameQuery = PredicateBuilder.True<Post>();

                string[] nameSplit = name.Split(' ');
                foreach (var item in nameSplit)
                {
                    nameQuery = nameQuery.And(p =>
                    p.Name.Contains(item) ||
                    p.PostLanguage.Any(ss => ss.Name.Contains(item) || (ss.Keywords.Contains(item)) || (ss.Summary.Contains(item))) ||
                    (p.Keywords.Contains(item)) ||
                    (p.Summary.Contains(item)));
                }
                MyQuery = MyQuery.And(nameQuery);
            }
            return MyQuery;
        }

        public PostVm GetByLabel(string label)
        {
            if (!string.IsNullOrEmpty(label))
            {
                var post = FirstOrDefault(s => s.PostLabel == label);
                if (post != null)
                {
                    return post.Map<Post, PostVm>();
                }
            }
            return null;
        }
        public List<Post> SearchBox(string name = null, int size = 5)
        {
            return Where(GetSearchQuery2(name: name, active: true/*,categoryLabel:"blog"*/)).OrderByDescending(s => s.ID).Take(size).ToList();
        }

        public List<HomePostCardVm> GetHomeCards(string categoryLabel, int take = 5, bool orderByShowNumber = false)
        {
            // Post has no Deleted column; Active is its publication/deletion flag.
            IQueryable<Post> query = _context.Post.AsNoTracking().Where(p => p.Active == true);

            if (!string.IsNullOrWhiteSpace(categoryLabel))
                query = query.Where(p => p.Category.Label == categoryLabel);

            var orderedQuery = orderByShowNumber
                ? query.OrderBy(p => p.ShowNumber.HasValue ? 0 : 1).ThenBy(p => p.ShowNumber).ThenByDescending(p => p.ID)
                : query.OrderByDescending(p => p.ID);

            var cards = orderedQuery.Take(take).Select(p => new HomePostCardVm
            {
                ID = p.ID,
                Name = p.Name,
                Summary = p.Summary,
                CategoryName = p.Category.Name,
                ImageUrl = p.Picture.Url,
                PostUrl = p.URL,
                CreateDateTime = p.CreateDateTime
            }).ToList();

            foreach (var card in cards)
            {
                card.PostUrl = new Post { ID = card.ID, Name = card.Name, URL = card.PostUrl }.GetLinkWithUrl();
                if (!string.IsNullOrWhiteSpace(card.ImageUrl))
                {
                    card.ImageUrl = card.ImageUrl.Replace(Enum_Code.SYSTEM_TYPE_PANEL.ToString(), BaseWebsite.PanelUrl);
                    if (!card.ImageUrl.StartsWith("http") && !card.ImageUrl.StartsWith("/"))
                        card.ImageUrl = "/" + card.ImageUrl;
                }
            }

            return cards;
        }
    }
}
