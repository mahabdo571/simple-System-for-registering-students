using System;

[Flags] 
public enum enPermissions
{
    None = 0,
    Read = 1,    
    Write = 2,  
    Delete = 4,  
    Modify = 8 
}

