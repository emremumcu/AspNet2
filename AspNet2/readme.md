# AspNet0 Readme

## Changing default SSL port number

IISExpress uses http.sys for its communication and it requires SSL ports to be registered as Administrator.
To avoid running Visual Studio as administrator, IIS Express reserves the port range 44300 - 44399 when it is installed.
As long as you select a port within this range (Properties->launchSettings.json->iisSettings->sslPort) you do not need to 
run IISExpressAdminCmd to register the URL.

## Razor syntax reference for ASP.NET Core

https://docs.microsoft.com/en-us/aspnet/core/mvc/views/razor?view=aspnetcore-3.1

    In Razor Views, implicit expressions cannot contain C# generics, as the characters 
    inside the brackets (<>) are interpreted as an HTML tag. 
    
    The following code is not valid:
    
    <p>@GenericMethod<int>()</p>

    Generic method calls must be wrapped in an explicit Razor expression or a Razor code block.

    
    Razor expression:
    -----------------

    Explicit Razor expressions consist of an @ symbol with balanced parenthesis: @()

    <p>@(GenericMethod<int>())</p>

    
    Razor code blocks:
    ------------------

    Razor code blocks start with @ and are enclosed by {}. Unlike expressions, C# code inside code blocks isn't rendered.
    Code blocks and expressions in a view share the same scope and are defined in the order they are written.

    @{
        var str = "This is a string.";
    }

    In code blocks, we can declare local functions with markup to serve as templating methods:

    @{
        void RenderName(string name)
        {
            <p>Name: <strong>@name</strong></p>
        }

        RenderName("Emre");        
    }

    
    Implicit transitions:
    ---------------------

    The default language in a code block is C#, but the Razor Page can transition back to HTML:

    @{
        var inCSharp = true;
        <p>Now in HTML, was in C# @inCSharp</p>
    }

    Explicit delimited transition:
    ------------------------------

    To define a subsection of a code block that should render HTML, surround the characters for 
    rendering with the Razor <text> tag:

    @for (var i = 0; i < people.Length; i++)
    {
        var person = people[i];
        <text>Name: @person.Name</text>
    }

    Use this approach to render HTML that isn't surrounded by an HTML tag. Without an HTML or 
    Razor tag, a Razor runtime error occurs.

    The <text> tag is useful to control whitespace when rendering content:

        * Only the content between the <text> tag is rendered.
        * No whitespace before or after the <text> tag appears in the HTML output.

    Explicit line transition:
    -------------------------

    To render the rest of an entire line as HTML inside a code block, use @: syntax:

    @for (var i = 0; i < people.Length; i++)
    {
        var person = people[i];
        @:Name: @person.Name
    }

    Without the @: in the code, a Razor runtime error is generated.

### New Caption