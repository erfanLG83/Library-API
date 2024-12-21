namespace Application.Users.Common;

public static class UserErrors
{
    public static Error UserIsNotAdmin = new("اکانت شما دسترسی ندارد", "User_Is_Not_Admin");
    public static Error PhoneNumberAlreadyExists = new("این شماره تلفن تکراری است، لطفا از یک شماره تلفن دیگر استفاده نمایید", "PhoneNumber_Already_Exist");
    public static Error VerificationTokenHasExpired = new("کد تایید منقضی شده است.", "Verification_Token_Has_Expired");
    public static Error UserNotFound = new("کاربر یافت نشد.", "User_Not_Found");
    public static Error PhoneNumberOrPasswordIsNotCorrect = new("نام کاربری یا رمز عبور صحیح نمی باشد.", "PhoneNumber_Or_Password_Is_Not_Correct");
    public static Error RefreshTokenIsNotValid = new("رفرش توکن معتبر نمی باشد.", "Refresh_Token_Is_Not_Valid");
    public static Error PreviousPasswordIsNotValid = new("رمز عبور قبلی صحیح نمی باشد.", "User_Can_Not_Change_Password_With_Current_Status");
    public static Error PhoneNumberIsNotValid = new("مقادیر ورودی معتبر نمی باشد.", "PhoneNumber_Is_Not_Valid");
    public static Error OtpCodeIsInvalid = new("کد ورود صحیح نمی باشد", "otp_code_is_invalid");
    public static Error OtpCodeHasExpired = new("کد ورود منقضی شده است", "otp_code_has_expired");
    public static Error CanNotResendOtpCode = new("برای ارسال مجدد کد ورود باید صبر کنید", "cant_resend_otpcode");
    public static Error PhoneNumberDoesNotExists = new("کاربری با این شماره تلفن وجود ندارد", "phone_number_does_not_exists");
    public static Error FirstNameAndLastNameIsRequired = new("نام و نام خانوادگی اجباری است", "firstName_lastName_is_required");
}