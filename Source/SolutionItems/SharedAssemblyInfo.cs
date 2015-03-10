using System.Reflection;

#if DEBUG
[assembly: AssemblyProduct("DoWorkGym (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("DoWorkGym (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription("by @mmo_80")]
[assembly: AssemblyCompany("Miguel Mendes")]
[assembly: AssemblyCopyright("Copyright © Miguel Mendes 2015")]
[assembly: AssemblyTrademark("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.3.26.*")]
[assembly: AssemblyFileVersion("0.3.26")]