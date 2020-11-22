﻿using Music.Data;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Music.Services
{


    public class AlbumService
    {
        private readonly Guid _userId;

        public AlbumService(Guid userId)
        {
            _userId = userId;
        }
        public bool CreateAlbum(AlbumCreate model)
        {
            var entity =
                new Album()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Genre = model.Genre,
                    ReleaseDate = model.ReleaseDate,

                     CreatedUtc = DateTimeOffset.Now
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Albums.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }
        public IEnumerable<AlbumListItem> GetAlbums()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Albums
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new AlbumListItem
                                {
                                    AlbumId = e.AlbumId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc
                                }
                        );

                return query.ToArray();
            }
        }
        public AlbumDetail GetAlbumById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Albums
                        .Single(e => e.AlbumId == id && e.OwnerId == _userId);
                return
                    new AlbumDetail
                    {
                        AlbumId = entity.AlbumId,
                        OwnerId = _userId,
                        Title = entity.Title,
                        Genre = entity.Genre,
                        ReleaseDate = entity.ReleaseDate,

                        CreatedUtc = DateTimeOffset.Now,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }

        }
        public bool UpdateAlbum(AlbumEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Albums
                        .Single(e => e.AlbumId == model.AlbumId && e.OwnerId == _userId);
                //entity.AlbumId = model.AlbumId;
                entity.Title = model.Title;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

       
        public bool DeleteAlbum(int songId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Albums
                        .Single(e => e.AlbumId == songId && e.OwnerId == _userId);

                ctx.Albums.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}