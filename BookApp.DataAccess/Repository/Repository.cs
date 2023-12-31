﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookApp.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace BookApp.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        void IRepository<T>.Add(T entity)
        {
            dbSet.Add(entity);
        }

        void IRepository<T>.Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        T IRepository<T>.Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            if (tracked)
            {
                IQueryable<T> query = dbSet;
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(item);
                    }
                }
                query = query.Where(filter);
                return query.FirstOrDefault();
            }
            else
            {
                IQueryable<T> query = dbSet.AsNoTracking();
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(item);
                    }
                }
                query = query.Where(filter);
                return query.FirstOrDefault();
            }

        }

        IEnumerable<T> IRepository<T>.GetAll(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {      
            if(filter != null)
            {
                IQueryable<T> query = dbSet;

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(item);
                    }
                }

                query = query.Where(filter);
                return query.ToList();
            }
            else
            {
                IQueryable<T> query = dbSet;
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(item);
                    }
                }
                return query.ToList();
            }
        }

        void IRepository<T>.RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
