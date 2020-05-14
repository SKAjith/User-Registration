# User Login and Registration using Verification Code
Here, I used Identity to Authenticate the user. 
Used .Net Core MVC, this can be used as a Web API too.
Here, I didn't use email functinality as because I don't have a SMTP server but did implemented Verification Code logic. A column named Verification got introduced in ASPNetUsers table in Identity DataBase.
No DB stored procedure were written. Created a Identity Database which gets auto created using Identity.
Data/VerificationUser.cs is the class used for Extended Identity fields.
Create 3 pages named Login.cshtml, Register.cshtml and UserSetails.cshtml under folder Views/Account.
Used jQuery for making the password visible.
Used bootstrap for designs.
I haven't checked-in folders like bin and obj.
Also, Solution file will not be available in my check-in
