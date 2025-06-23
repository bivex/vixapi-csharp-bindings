using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace VixBindings.Samples {
public class Sample {
    // Replace with your actual values
    private const string HostName = "localhost"; // Or your ESXi/vCenter IP/hostname
    private const int HostPort = 0; // Default port
    private const string UserName = ""; // Your host username
    private const string Password = ""; // Your host password
    private const string VmxFilePath = "C:\\Users\\Admin\\Documents\\Virtual Machines\\WindowsDev\\WindowsDev.vmx"; // Path to your VM's .vmx file
    private const string GuestUserName = "worker"; // Guest OS username
    private const string GuestPassword = ""; // Guest OS password
    private const string HostFilePath = "C:\\temp\\host_file.txt"; // File on host to copy
    private const string GuestFilePath = "C:\\temp\\guest_file.txt"; // Destination path in guest
    private const string CopiedGuestFilePath = "C:\\temp\\guest_file_copied.txt"; // File to copy from guest
    private const string CopiedHostFilePath = "C:\\temp\\host_file_copied.txt"; // Destination path on host

    private static AutoResetEvent _jobEvent = new AutoResetEvent ( false );
    private static ulong _jobResult = VixApi.VIX_OK;

    public static void Main ( string[] args )
    {
        int hostHandle = VixApi.VIX_INVALID_HANDLE;
        int vmHandle = VixApi.VIX_INVALID_HANDLE;
        int jobHandle = VixApi.VIX_INVALID_HANDLE;

        try
        {
            Console.WriteLine ( "Connecting to host..." );
            jobHandle = VixApi.VixHost_Connect (
                            VixApi.VIX_API_VERSION,
                            VixApi.VixServiceProvider.VMwareWorkstation,
                            HostName,
                            HostPort,
                            UserName,
                            Password,
                            VixApi.VixHostOptions.VerifySslCert,
                            VixApi.VIX_INVALID_HANDLE,
                            JobCallback,
                            IntPtr.Zero );

            ulong connectionResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_JOB_RESULT_HANDLE, out hostHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != connectionResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( connectionResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to connect to host: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
                return;
            }
            Console.WriteLine ( "Connected to host." );
            VixApi.Vix_ReleaseHandle ( jobHandle );

            Console.WriteLine ( "Opening VM..." );
            jobHandle = VixApi.VixHost_OpenVM (
                            hostHandle,
                            VmxFilePath,
                            VixApi.VixVMOpenOptions.Normal,
                            VixApi.VIX_INVALID_HANDLE,
                            JobCallback,
                            IntPtr.Zero );

            ulong openVmResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_JOB_RESULT_HANDLE, out vmHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != openVmResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( openVmResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to open VM: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
                return;
            }
            Console.WriteLine ( "VM opened." );
            VixApi.Vix_ReleaseHandle ( jobHandle );

            Console.WriteLine ( "Waiting for tools in guest..." );
            jobHandle = VixApi.VixVM_WaitForToolsInGuest (
                            vmHandle,
                            600, // Timeout in seconds
                            JobCallback,
                            IntPtr.Zero );

            _jobResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != _jobResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( _jobResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to wait for tools: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
                return;
            }
            Console.WriteLine ( "Tools are running in guest." );
            VixApi.Vix_ReleaseHandle ( jobHandle );

            Console.WriteLine ( "Logging in to guest OS..." );
            jobHandle = VixApi.VixVM_LoginInGuest (
                            vmHandle,
                            GuestUserName,
                            GuestPassword,
                            0, // Options
                            JobCallback,
                            IntPtr.Zero );

            _jobResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != _jobResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( _jobResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to login to guest: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
                return;
            }
            Console.WriteLine ( "Logged in to guest OS." );
            VixApi.Vix_ReleaseHandle ( jobHandle );

            // Create a dummy file on the host for copying
            System.IO.File.WriteAllText ( HostFilePath, "This is a test file from the host." );
            Console.WriteLine ( $"Created dummy file on host: {HostFilePath}" );

            Console.WriteLine ( $"Copying file from host to guest: {HostFilePath} -> {GuestFilePath}..." );
            jobHandle = VixApi.VixVM_CopyFileFromHostToGuest (
                            vmHandle,
                            HostFilePath,
                            GuestFilePath,
                            0, // Options
                            VixApi.VIX_INVALID_HANDLE,
                            JobCallback,
                            IntPtr.Zero );

            _jobResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != _jobResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( _jobResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to copy file from host to guest: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
            }
            else
            {
                Console.WriteLine ( "File copied from host to guest successfully." );
            }
            VixApi.Vix_ReleaseHandle ( jobHandle );

            Console.WriteLine ( $"Copying file from guest to host: {CopiedGuestFilePath} -> {CopiedHostFilePath}..." );
            jobHandle = VixApi.VixVM_CopyFileFromGuestToHost (
                            vmHandle,
                            CopiedGuestFilePath,
                            CopiedHostFilePath,
                            0, // Options
                            VixApi.VIX_INVALID_HANDLE,
                            JobCallback,
                            IntPtr.Zero );

            _jobResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != _jobResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( _jobResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to copy file from guest to host: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
            }
            else
            {
                Console.WriteLine ( "File copied from guest to host successfully." );
            }
            VixApi.Vix_ReleaseHandle ( jobHandle );

            Console.WriteLine ( "Logging out from guest OS..." );
            jobHandle = VixApi.VixVM_LogoutFromGuest (
                            vmHandle,
                            JobCallback,
                            IntPtr.Zero );

            _jobResult = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_NONE );
            if ( VixApi.VIX_OK != _jobResult )
            {
                IntPtr errorTextPtr = VixApi.Vix_GetErrorText ( _jobResult, ( string? ) null );
                string errorMessage = Marshal.PtrToStringAnsi ( errorTextPtr ) ?? "Unknown error";
                Console.WriteLine ( $"Failed to logout from guest: {errorMessage}" );
                VixApi.Vix_FreeBuffer ( errorTextPtr );
            }
            else
            {
                Console.WriteLine ( "Logged out from guest OS." );
            }
            VixApi.Vix_ReleaseHandle ( jobHandle );
        }
        catch ( Exception ex )
        {
            Console.WriteLine ( $"An unexpected error occurred: {ex.Message}" );
        }
        finally
        {
            if ( vmHandle != VixApi.VIX_INVALID_HANDLE )
            {
                VixApi.Vix_ReleaseHandle ( vmHandle );
                Console.WriteLine ( "VM handle released." );
            }
            if ( hostHandle != VixApi.VIX_INVALID_HANDLE )
            {
                VixApi.VixHost_Disconnect ( hostHandle );
                Console.WriteLine ( "Disconnected from host." );
            }
            if ( jobHandle != VixApi.VIX_INVALID_HANDLE )
            {
                VixApi.Vix_ReleaseHandle ( jobHandle );
                Console.WriteLine ( "Job handle released." );
            }
        }
    }

    private static void JobCallback ( int handle, int eventType, int moreEventInfo, IntPtr clientData )
    {
        if ( eventType == ( int ) VixApi.VixEventType.JobCompleted )
        {
            _jobResult = VixApi.VixJob_GetError ( handle );
            _jobEvent.Set();
        }
    }

    private static int GetJobResultHandle ( int jobHandle )
    {
        int resultHandle = VixApi.VIX_INVALID_HANDLE;
        ulong err = VixApi.VixJob_Wait ( jobHandle, VixApi.VIX_PROPERTY_JOB_RESULT_HANDLE, out resultHandle, VixApi.VIX_PROPERTY_NONE );
        if ( VixApi.VIX_OK == err )
        {
            return resultHandle;
        }
        return VixApi.VIX_INVALID_HANDLE;
    }
}
}