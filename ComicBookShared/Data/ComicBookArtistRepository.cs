using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBookArtistRepository : BaseRepository<ComicBookArtist>
    {

        public ComicBookArtistRepository(Context context) : base(context)
        {
        }

        public override ComicBookArtist Get(int id, bool includeRelatedEntities = true)
        {
            var comicBookArtists = Context.ComicBookArtists.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBookArtists = comicBookArtists
                    .Include(cba => cba.Artist)
                    .Include(cba => cba.Role)
                    .Include(cba => cba.ComicBook.Series);
            }

            return comicBookArtists
                .Where(cba => cba.Id == id)
                .SingleOrDefault();
        }

        // Since our web app doesn't need a method to get a list of comic book artists, we can just leave the step-out method that throws a new instance of the NotImplementedException class.
        public override IList<ComicBookArtist> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
