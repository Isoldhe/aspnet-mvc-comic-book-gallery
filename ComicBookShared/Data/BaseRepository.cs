using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public abstract class BaseRepository<TEntity>
        // When we call the Set method with our TEntity generic type parameter, we get a build error. 
        // The type 'TEntity' must be a reference type in order to use it as a parameter 'TEntity' in the generic type or method 'DbContext.set. 
        // This means that the Set method is designed to only work with reference types. Types defined using classes and not value types like int or Bool. 
        // Remember that our TEntity generic type parameter currently allows any of the available .NET types. We can constrain our generic type parameter by using the where keyword. 
        // where, followed by the genetic type parameter to constrain, TEntity : Then the constrain we want to use, class, which constrains the genetic type parameter to reference types. 
        // Other constrains are available, including struct and new. struct would constrain the generic type parameter to value types. 
        // And new would constrain the generic type parameter to reference types that define a default constructor. 
        where TEntity : class 
    {
        protected Context Context { get; private set; }

        protected BaseRepository(Context context)
        {
            Context = context;
        }

        // object is used as return type because this class should be able to be used by both ComicBookRepo and ComicBookArtistRepo
        // So we can't return a ComicBook or ComicBookArtist. That's when you can just return an object:
        //public object Get(int id)
        //{
        //return null;
        //}

        // But then the caller would need to explicitly cast the return value to whatever entity type they were working with. 
        // That'd definitely be less than ideal. This is where generics come to our rescue: <TEntity>
        // While it's not required, it's a convention to name the generic type parameter
        // starting with a T followed by a description of the type.
        // In our case, this is the Entity type, so TEntity seems like an appropriate name.
        // And then we can also return that type:
        //public TEntity Get<TEntity>(int id)
        //{
        //    return default(TEntity);
        //}

        // If you define the generic type parameter at the class level, like we did at the top, you don't have to do that in every method, as that is the default:
        public abstract TEntity Get(int id, bool includeRelatedEntities = true);

        public abstract IList<TEntity> GetList();

        public void Add(TEntity entity)
        {
            // When we called the Set method with our TEntity generic type parameter, we got a build error. That's why we added the where clause at the top (see explanation there)
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var set = Context.Set<TEntity>();
            var entity = set.Find(id);
            set.Remove(entity);
            Context.SaveChanges();
        }
    }
}