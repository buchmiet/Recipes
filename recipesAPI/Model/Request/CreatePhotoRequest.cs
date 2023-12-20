using FluentValidation;
using recipesCommon.Interfaces;
using static recipesCommon.DataAccess.RecipesDbContext;

namespace recipesCommon.Model.Request
{
    public class CreatePhotoRequest
    {
        public string Address { get; set; }
    }

    public class CreatePhotoRequestValidator : AbstractValidator<CreatePhotoRequest>
    {
        IEntityService<Photo> _photoService;
        public CreatePhotoRequestValidator(IEntityService<Photo> photoService)
        {
            _photoService = photoService;
            RuleFor(request => request.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters")
                  .MustAsync(BeUniqueAddress).WithMessage("Address must be unique");
        }

        private async Task<bool> BeUniqueAddress(string address, CancellationToken cancellationToken)
        {
            // Sprawdź, czy istnieje już zdjęcie o podanym adresie
            var existingPhoto = await _photoService.GetOneAsync(p=>p.Address.Equals(address));

            // Jeśli istnieje, to adres nie jest unikalny
            return existingPhoto == null;
        }

    }

}
