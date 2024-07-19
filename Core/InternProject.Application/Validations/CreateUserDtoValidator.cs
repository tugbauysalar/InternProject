using FluentValidation;
using InternProject.Application.DTOs;

namespace InternProject.Application.Validations
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Ad-Soyad alanı boş bırakılamaz!")
            .NotNull().WithMessage("Ad-Soyad alanı boş bırakılamaz!");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Kullanıcı adı alanı boş bırakılamaz!")
                .NotNull().WithMessage("Kullanıcı adı alanı boş bırakılamaz!");
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta alanı boş bırakılamaz!")
                .NotNull().WithMessage("E-posta alanı boş bırakılamaz!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçerli bir email adresi girin!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş bırakılamaz!")
                .NotNull().WithMessage("Şifre alanı boş bırakılamaz!");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı!")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermeli!")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermeli!")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermeli!")
                .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermeli!");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password)
                .WithMessage("Şifreler eşleşmiyor!");
        }
    }
}
