﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.DataAccess.Repository.IRepository
{
    public interface IUniteOfWork
    {
        public ICategoryRepository Category { get; }
        public void Save();
    }
}