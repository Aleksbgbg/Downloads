namespace Downloads.Tests.Api
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Moq;

    public static class Utils
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(ICollection<T> sourceList)
                where T : class
        {
            IQueryable<T> sourceListQueryable = sourceList.AsQueryable();

            Mock<DbSet<T>> dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(dbSet => dbSet.Provider).Returns(sourceListQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(dbSet => dbSet.Expression).Returns(sourceListQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(dbSet => dbSet.ElementType).Returns(sourceListQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(dbSet => dbSet.GetEnumerator()).Returns(() => sourceListQueryable.GetEnumerator());

            dbSetMock.Setup(dbSet => dbSet.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);

            return dbSetMock.Object;
        }
    }
}