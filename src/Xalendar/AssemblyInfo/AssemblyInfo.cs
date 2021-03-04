using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xalendar.View.Controls;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Xalendar.Tests")]

//Custom xaml schema <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/custom-namespace-schemas#defining-a-custom-namespace-schema"/>
[assembly: XmlnsDefinition("http://xalendar.com/schemas/xaml", "Xalendar.View.Controls")]
[assembly: XmlnsDefinition("http://xalendar.com/schemas/xaml", "Xalendar.View.Themes")]
[assembly: XmlnsDefinition("http://xalendar.com/schemas/xaml", "Xalendar.Api.Formatters")]

//Recommended prefix <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/custom-prefix"/>
[assembly: XmlnsPrefix("http://xalendar.com/schemas/xaml", "xal")]

//Xaml compilation <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/xamlc"/>
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
