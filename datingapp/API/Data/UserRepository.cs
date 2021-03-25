using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(x => x.Username == username)
           .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMemberAsync(UserParams userParms)
        {
            var query= _context.Users.AsQueryable();
            
           
            query=query.Where(u=>u.Username!=userParms.CurrentUsername);
            query=query.Where(u=>u.Gender==userParms.Gender);
             var MinDob=DateTime.Today.AddYears(-userParms.MaxAge-1);
            var MaxDob=DateTime.Today.AddYears(-userParms.MinAge);
           query=query.Where(u=>u.DateOfBirth>=MinDob && u.DateOfBirth<=MaxDob);

           query=userParms.OrderBy switch
           {
                "created"=>query.OrderByDescending(u=>u.Created),
                _=>query.OrderByDescending(u=>u.LastActive)
           };
            return await PagedList<MemberDto>.CreateAsync(query
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .AsNoTracking(),userParms.PageNumber,userParms.PageSize);
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
             .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();

        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

    
    }
}