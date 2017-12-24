REQUIRED COMMENTS FOR COMMAND/QUERY MESSAGES

Because the commands and queries all return a CommmonResult object that may contain different types
of data depending on the result of the command or query, it is MANDATORY that a class-summary 
comment be created for all command and query messages.

The comment must include what is contained in the dynamic property CommonResult.Data if the 
command/query succeeds, and it must also outline the types of returns in the event of any
Status other than Success.

Examples:

command SignInByPassword:
	/// <summary>
    /// Sign in a local user by password. If successful the returned CommonResult.Data 
    /// contains a single SignInResult object. On failure CommonResult.Data contains an
    /// error message string and FlashMessage contains a general error notice string.
    /// </summary>

command RegisterLocalUser:
    /// <summary>
    /// Register a local user with the Identity system. If successful the returned CommonResult.Data 
    /// contains a single AppUser object. On failure CommonResult.Data contains a List<string> 
    /// of error messages and FlashMessage contains a general error notice string.
    /// </summary>

Command WriteLogMessage:
    /// <summary>
    /// This will write a message to the current logger, primarily for testing the command system. 
    /// If no exception the returned CommonResult contains only StatusResult.Success. If an exception
    /// is caught CommonResult.Flash message contains the exception description and CommonResult.Data
    /// contains the exception object.
    /// </summary>