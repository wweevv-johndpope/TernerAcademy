using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.StudentPortal.Categories.Commands.SetPreferences
{
    public class SetCategoryPreferencesCommand : IRequest<IResult>
    {
        public List<int> CategoryIds { get; set; } = new();

        public class SetCategoryPreferencesCommandHandler : IRequestHandler<SetCategoryPreferencesCommand, IResult>
        {
            private readonly ICallContext _context;
            private readonly IApplicationDbContext _dbContext;

            public SetCategoryPreferencesCommandHandler(ICallContext context, IApplicationDbContext dbContext)
            {
                _context = context;
                _dbContext = dbContext;
            }

            public async Task<IResult> Handle(SetCategoryPreferencesCommand request, CancellationToken cancellationToken)
            {
                request.CategoryIds ??= new();

                if (!request.CategoryIds.Any()) return await Result.FailAsync("Category Preferences is required.");

                var currentCategoryPreferences = await _dbContext.StudentCategoryPreferences.AsQueryable().Where(x => x.StudentId == _context.UserId).ToListAsync();

                foreach (var currentCategoryPreference in currentCategoryPreferences)
                {
                    if (!request.CategoryIds.Any(x => x == currentCategoryPreference.CategoryId))
                    {
                        _dbContext.StudentCategoryPreferences.Remove(currentCategoryPreference);
                    }
                }

                await _dbContext.SaveChangesAsync();

                var categories = await _dbContext.Categories.AsQueryable().ToListAsync();

                currentCategoryPreferences = await _dbContext.StudentCategoryPreferences.AsQueryable().Where(x => x.StudentId == _context.UserId).ToListAsync();

                foreach (var categoryId in request.CategoryIds)
                {
                    if (!currentCategoryPreferences.Any(x => x.CategoryId == categoryId) && categories.Any(x => x.Id == categoryId))
                    {
                        _dbContext.StudentCategoryPreferences.Add(new StudentCategoryPreference() { StudentId = _context.UserId, CategoryId = categoryId });
                    }
                }

                await _dbContext.SaveChangesAsync();

                return await Result.SuccessAsync();
            }
        }
    }
}
