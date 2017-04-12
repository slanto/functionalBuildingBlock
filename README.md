Functional building blocs


Install-Package NullGuard.Fody

and add IncludeDebugAssert to configuration in order to always throw exception in debug mode

<?xml version="1.0" encoding="utf-8"?>
<Weavers>
  <NullGuard IncludeDebugAssert="false" />
</Weavers>
