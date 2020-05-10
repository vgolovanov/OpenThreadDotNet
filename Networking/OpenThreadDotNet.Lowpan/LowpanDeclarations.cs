namespace OpenThreadDotNet.Networking.Lowpan
{
    public enum PowerState
    {
        On = 0,
        LowPower = 1,
        Off = 2,
    }

    public enum State
    {
        Detached = 0,
        Child = 1,
        Router = 2,
        Leader = 3,
    }

    public enum InterfaceType
    {       
        BOOTLOADER = 0,
        ZIGBEE_IP = 2,
        THREAD = 3,
    }

    public enum LastStatus
    {
        Ok = 0,  ///< Operation has completed successfully.
        Failure = 1,  ///< Operation has failed for some undefined reason.
        Unimplemented = 2,  ///< Given operation has not been implemented.
        Invalid_Argument = 3,  ///< An argument to the operation is invalid.
        Invalid_State = 4,  ///< This operation is invalid for the current device state.
        Invalid_Command = 5,  ///< This command is not recognized.
        Invalid_Interface = 6,  ///< This interface is not supported.
        Internal_error = 7,  ///< An internal runtime error has occured.
        Security_error = 8,  ///< A security/authentication error has occured.
        Parse_error = 9,  ///< A error has occured while parsing the command.
        In_progress = 10, ///< This operation is in progress.
        Nomem = 11, ///< Operation prevented due to memory pressure.
        Busy = 12, ///< The device is currently performing a mutually exclusive operation
        Prop_not_found = 13, ///< The given property is not recognized.
        Dropped = 14, ///< A/The packet was dropped.
        Empty = 15, ///< The result of the operation is empty.
        Cmd_too_big = 16, ///< The command was too large to fit in the internal buffer.
        No_ack = 17, ///< The packet was not acknowledged.
        Cca_failure = 18, ///< The packet was not sent due to a CCA failure.
        Already = 19, ///< The operation is already in progress.
        Item_not_found = 20, ///< The given item could not be found.
        Invalid_command_for_prop = 21, ///< The given command cannot be performed on this property.
        STATUS_RESET_POWER_ON= 112,
        STATUS_RESET_EXTERNAL=113,
        Status_Reset_Software=114,
        STATUS_RESET_FAULT=115,
        STATUS_RESET_CRASH=116,
        STATUS_RESET_ASSERT=117,
        STATUS_RESET_OTHER=118,
        STATUS_RESET_UNKNOWN=119,
        STATUS_RESET_WATCHDOG=120
    }
}
