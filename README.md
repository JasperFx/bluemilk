# bluemilk
Runtime Codegen Chicanery with Roslyn

It's a little hard to explain, but BlueMilk is the runtime code generation infrastructure 
that originated in [Jasper](http://jasperfx.github.io). 

It's also meant to be a new IoC
tool to replace the venerable [StructureMap](http://structuremap/structuremap) container, but use
the runtime code generation to be the fastest IoC tool possible. The scope of BlueMilk will be a subset of
StructureMap, with an emphasis on being compliant and usable within ASP.Net Core applications and the 
merciless removal of StructureMap features that gave users way too much rope with which to hang themselves
in code.
