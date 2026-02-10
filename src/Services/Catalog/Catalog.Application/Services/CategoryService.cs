using Catalog.Application.DTOs;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using System.Text.Json;
using Library.Shared.Events;

namespace Catalog.Application.Services;

public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEventPublisher _eventPublisher;

    public CategoryService(ICategoryRepository categoryRepository, IEventPublisher eventPublisher)
    {
        _categoryRepository = categoryRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<CategoryDto> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");
        
        return MapToDto(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return categories.Select(MapToDto);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name
        };

        category = await _categoryRepository.AddAsync(category);

        await _eventPublisher.PublishAsync(new CategoryCreated
        {
            CategoryId = category.Id,
            Name = category.Name,
            EntityId = category.Id.ToString(),
            EntityType = "Category",
            ActionType = "Created",
            Payload = JsonSerializer.Serialize(category)
        });

        return MapToDto(category);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        category.Name = dto.Name;
        await _categoryRepository.UpdateAsync(category);

        await _eventPublisher.PublishAsync(new CategoryUpdated
        {
            CategoryId = category.Id,
            Name = category.Name,
            EntityId = category.Id.ToString(),
            EntityType = "Category",
            ActionType = "Updated",
            Payload = JsonSerializer.Serialize(category)
        });

        return MapToDto(category);
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        await _categoryRepository.DeleteAsync(id);

        await _eventPublisher.PublishAsync(new CategoryDeleted
        {
            CategoryId = id,
            EntityId = id.ToString(),
            EntityType = "Category",
            ActionType = "Deleted",
            Payload = JsonSerializer.Serialize(new { CategoryId = id })
        });
    }

    private static CategoryDto MapToDto(Category category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
