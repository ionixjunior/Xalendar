using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xalendar.View.Controls;
using System.Runtime.CompilerServices;

//Linker safe
[assembly: InternalsVisibleTo("Xalendar.Api.Tests")]
[assembly: InternalsVisibleTo("Xalendar.View.Tests")]

//Custom xaml schema <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/custom-namespace-schemas#defining-a-custom-namespace-schema"/>
[assembly: XmlnsDefinition("http://xalendar.com/schemas/xaml", "Xalendar.View.Controls")]
[assembly: XmlnsDefinition("http://xalendar.com/schemas/xaml", "Xalendar.View.Themes")]

//Recommended prefix <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/custom-prefix"/>
[assembly: XmlnsPrefix("http://xalendar.com/schemas/xaml", "xal")]

//Xaml compilation <see href="https://docs.microsoft.com/pt-br/xamarin/xamarin-forms/xaml/xamlc"/>
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
