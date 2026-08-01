# FluentValidation in ASP.NET Core Web API

## What is FluentValidation?

**FluentValidation** is a popular .NET library used to validate models and DTOs using a clean, readable, and maintainable syntax.

Instead of using Data Annotations like `[Required]` or `[StringLength]`, validation rules are written in separate validator classes.

---

## Why Use FluentValidation?

| **Benefit** | **Description** |
|--------------|-----------------|
| ✅ Clean Code | Keeps validation logic separate from DTOs. |
| 🔄 Reusable | Validation rules can be reused across the application. |
| 📖 Readable | Uses a fluent, easy-to-read syntax. |
| 🛠️ Easy to Maintain | Validation changes don't require modifying DTOs. |
| 🚀 Advanced Validation | Supports custom rules, conditional validation, and async validation. |

---

## Install Package

```bash
dotnet add package FluentValidation.AspNetCore
```
---
## Register FluentValidation in Program.cs

```csharp
builder.Services.AddFluentValidationAutoValidation();
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
```

---
> ```ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;``` stops validation after the first failed rule for each property, so only one validation message is returned per field.
<br>

> ``` builder.Services.AddFluentValidationAutoValidation(); ``` Once registered, FluentValidation runs automatically before the controller action executes. If validation fails, ASP.NET Core returns a 400 Bad Request with validation errors.

<br>

>```AddValidatorsFromAssemblyContaining<T>() ``` Automatically registers all validators in the assembly containing < T >

---

## Create DTO

```csharp
public class CreatedUsersDto
{
    public string UserName {  get; set; } = string.Empty;

    public int RoleId { get; set; } 

    public string EmailAddress { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}
```

---

## Create Validator

```csharp
using DEMO.DTOs.UsersDtos;
using FluentValidation;

namespace DEMO.Validators.UserValidators
{
    public class CreateUserValidator : AbstractValidator<CreatedUsersDto>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty()
                    .WithMessage("User name is required.").
                   MaximumLength(20)
                    .WithMessage("User name cannot exceed 20 characters.");

            RuleFor(x => x.EmailAddress)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Email address is required.")
                .EmailAddress()
                    .WithMessage("Please enter a valid email address.");

            RuleFor(x => x.RoleId)
                .Cascade(CascadeMode.Stop)
                .GreaterThan(0)
                    .WithMessage("Please select a valid role.");

            RuleFor(x => x.Password)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage("Password is required.")
                .MinimumLength(6)
                    .WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100)
                    .WithMessage("Password cannot exceed 100 characters long.");
        }
    }
}
```

---

## Example Request


## Validation Test Cases

### Test Case 1: Multiple Validation Errors

**Request**

```json
{
  "userName": "",
  "emailAddress": "abc",
  "roleId": 0,
  "password": "123"
}
```

**Response**

```json
{
  "errors": {
    "UserName": [
      "User name is required."
    ],
    "EmailAddress": [
      "Please enter a valid email address."
    ],
    "RoleId": [
      "Please select a valid role."
    ],
    "Password": [
      "Password must be at least 6 characters long."
    ]
  }
}
```

---

### Test Case 2: Empty User Name

**Request**

```json
{
  "userName": "",
  "emailAddress": "daksh@gmail.com",
  "roleId": 1,
  "password": "123456"
}
```

**Response**

```json
{
  "errors": {
    "UserName": [
      "User name is required."
    ]
  }
}
```

---

### Test Case 3: User Name Exceeds Maximum Length

**Request**

```json
{
  "userName": "DakshBhalara_Developer",
  "emailAddress": "daksh@gmail.com",
  "roleId": 1,
  "password": "123456"
}
```

**Response**

```json
{
  "errors": {
    "UserName": [
      "User name cannot exceed 20 characters."
    ]
  }
}
```

---

### Test Case 4: Invalid Email Address

**Request**

```json
{
  "userName": "Daksh",
  "emailAddress": "dakshgmail.com",
  "roleId": 1,
  "password": "123456"
}
```

**Response**

```json
{
  "errors": {
    "EmailAddress": [
      "Please enter a valid email address."
    ]
  }
}
```

---

### Test Case 5: Invalid RoleId (0)

**Request**

```json
{
  "userName": "Daksh",
  "emailAddress": "daksh@gmail.com",
  "roleId": 0,
  "password": "123456"
}
```

**Response**

```json
{
  "errors": {
    "RoleId": [
      "Please select a valid role."
    ]
  }
}
```

---

### Test Case 6: Invalid RoleId (Negative)

**Request**

```json
{
  "userName": "Daksh",
  "emailAddress": "daksh@gmail.com",
  "roleId": -1,
  "password": "123456"
}
```

**Response**

```json
{
  "errors": {
    "RoleId": [
      "Please select a valid role."
    ]
  }
}
```

---

### Test Case 7: Valid Request

**Request**

```json
{
  "userName": "Daksh",
  "emailAddress": "daksh@gmail.com",
  "roleId": 1,
  "password": "123456"
}
```

**Response**

```json
{
  "message": "User created successfully."
}
```
--- 
### Restrict `RoleName` to Allowed Values

Use the `Must()` method to allow only specific role names.

```csharp
public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.RoleName)
            .NotEmpty()
                .WithMessage("Role name is required.")
            .MaximumLength(20)
                .WithMessage("Role name cannot exceed 20 characters.")
            .Must(role => new[] { "Admin", "Student", "Faculty", "HOD", "Principal" }
                .Contains(role, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Role name must be one of: Admin, Student, Faculty, HOD, Principal.");
    }
}
```

### How It Works

- `NotEmpty()` → Ensures the role name is not empty.
- `MaximumLength(20)` → Limits the role name to 20 characters.
- `Must()` → Allows only the predefined role names.
- `StringComparer.OrdinalIgnoreCase` → Makes the validation case-insensitive (e.g., `admin`, `ADMIN`, and `Admin` are all valid).

---

### Allowed Role Names

| ✅ Valid Role Names |
|---------------------|
| Admin |
| Student |
| Faculty |
| HOD |
| Principal |

---

### Valid Request

```json
{
  "roleName": "Admin",
  "roleDescription": "Administrator with full access"
}
```

---

### Invalid Request

```json
{
  "roleName": "Manager",
  "roleDescription": "Manager role"
}
```

### Validation Response

```json
{
  "errors": {
    "RoleName": [
      "Role name must be one of: Admin, Student, Faculty, HOD, Principal."
    ]
  }
}
```
---
## Common Validation Rules

| **Rule** | **Description** |
|-----------|-----------------|
| `NotEmpty()` | Value cannot be empty. |
| `NotNull()` | Value cannot be null. |
| `Length(min, max)` | Value must be within the specified length. |
| `MinimumLength(n)` | Minimum number of characters. |
| `MaximumLength(n)` | Maximum number of characters. |
| `EmailAddress()` | Validates email format. |
| `Matches()` | Validates using a regular expression. |
| `Equal()` | Value must match another value. |
| `NotEqual()` | Value must not match another value. |
| `InclusiveBetween()` | Value must be within a specified range. |
| `GreaterThan()` | Value must be greater than the specified value. |
| `LessThan()` | Value must be less than the specified value. |
| `Must()` | Custom validation rule. |

---

## Data Annotations vs FluentValidation

| **Data Annotations** | **FluentValidation** |
|----------------------|----------------------|
| Validation inside DTO | Validation in a separate class |
| Limited validation rules | Supports advanced validation |
| Less reusable | Highly reusable |
| Harder to maintain in large projects | Easy to maintain |
| Suitable for small projects | Recommended for medium and large projects |

--- 
# FluentValidation Methods (ASP.NET Core Web API)

## Basic Validation Methods

| Method | Use For | Example |
|--------|---------|---------|
| `NotEmpty()` | Required field | `.NotEmpty()` |
| `NotNull()` | Value cannot be null | `.NotNull()` |
| `Empty()` | Must be empty | `.Empty()` |
| `Null()` | Must be null | `.Null()` |
| `Length(min, max)` | String length | `.Length(3, 20)` |
| `MinimumLength(n)` | Minimum characters | `.MinimumLength(6)` |
| `MaximumLength(n)` | Maximum characters | `.MaximumLength(100)` |
| `Equal(value)` | Equal to value | `.Equal("Admin")` |
| `NotEqual(value)` | Not equal to value | `.NotEqual("Guest")` |
| `GreaterThan(value)` | Greater than value | `.GreaterThan(0)` |
| `GreaterThanOrEqualTo(value)` | Greater than or equal | `.GreaterThanOrEqualTo(18)` |
| `LessThan(value)` | Less than value | `.LessThan(100)` |
| `LessThanOrEqualTo(value)` | Less than or equal | `.LessThanOrEqualTo(100)` |
| `InclusiveBetween(min, max)` | Between (inclusive) | `.InclusiveBetween(1, 100)` |
| `ExclusiveBetween(min, max)` | Between (exclusive) | `.ExclusiveBetween(1, 100)` |
| `EmailAddress()` | Validate email | `.EmailAddress()` |
| `Matches(regex)` | Regular expression | `.Matches(@"^[A-Za-z]+$")` |
| `CreditCard()` | Credit card format | `.CreditCard()` |
| `IsInEnum()` | Enum value validation | `.IsInEnum()` |
| `PrecisionScale(p, s, ignoreTrailingZeros)` | Decimal precision & scale | `.PrecisionScale(10, 2, false)` |
| `Must()` | Custom validation | `.Must(x => x > 0)` |
| `MustAsync()` | Async custom validation | `.MustAsync(...)` |
| `Custom()` | Complex custom validation | `.Custom(...)` |
| `CustomAsync()` | Async complex validation | `.CustomAsync(...)` |

---

## Conditional Validation

| Method | Purpose |
|--------|---------|
| `When()` | Apply rule only if condition is true |
| `Unless()` | Skip rule if condition is true |
| `Otherwise()` | Execute alternative rules |
| `DependentRules()` | Execute dependent rules after previous rule passes |

---

## Collection & Nested Object Validation

| Method | Purpose |
|--------|---------|
| `RuleForEach()` | Validate every item in a collection |
| `ForEach()` | Apply rules to each collection item |
| `SetValidator()` | Use another validator for child object |
| `ChildRules()` | Define child object rules inline |

---

## Rule Flow Control

| Method | Purpose |
|--------|---------|
| `Cascade()` | Configure validation flow |
| `Cascade(CascadeMode.Stop)` | Stop after first failure |
| `Cascade(CascadeMode.Continue)` | Execute all validations |

---

## Error Message & Metadata

| Method | Purpose |
|--------|---------|
| `WithMessage()` | Custom error message |
| `WithName()` | Custom display name |
| `OverridePropertyName()` | Change property name in response |
| `WithErrorCode()` | Custom error code |
| `WithSeverity()` | Set validation severity |
| `WithState()` | Attach custom state |

---

## RuleSet & Reusability

| Method | Purpose |
|--------|---------|
| `RuleSet()` | Group validation rules |
| `Include()` | Include another validator |
| `IncludeRules()` | Include rules from another RuleSet |

---

## Common Rule Builders

| Method | Purpose |
|--------|---------|
| `RuleFor()` | Create validation rule for a property |
| `Transform()` | Transform property before validation |

---

## Most Used Methods (95% in Web API)

- `RuleFor()`
- `NotEmpty()`
- `NotNull()`
- `Length()`
- `MinimumLength()`
- `MaximumLength()`
- `EmailAddress()`
- `Matches()`
- `GreaterThan()`
- `GreaterThanOrEqualTo()`
- `LessThan()`
- `InclusiveBetween()`
- `Equal()`
- `NotEqual()`
- `Must()`
- `RuleForEach()`
- `SetValidator()`
- `When()`
- `Cascade()`
- `WithMessage()`
- `IsInEnum()`


