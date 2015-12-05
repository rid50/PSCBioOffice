Public Class GenericBinder(Of T)
    Inherits System.Runtime.Serialization.SerializationBinder

    ' public class GenericBinder<T> : System.Runtime.Serialization.SerializationBinder
    ' <param name="assemblyName">eg. App_Code.y4xkvcpq, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null</param>
    ' <param name="typeName">eg. String</param>
    ' <returns>Type for the deserializer to use</returns>
    Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As Type
        ' We're going to ignore the assembly name, and assume it's in the same assembly 
        ' that <T> is defined (it's either T or a field/return type within T anyway)
        Dim typeInfo As String() = typeName.Split(".")
        Dim isSystem As Boolean = (typeInfo(0).ToString() = "System")
        Dim className As String = typeInfo(typeInfo.Length - 1)

        ' noop is the default, returns what was passed in
        Dim toReturn As Type = Nothing
        Try
            toReturn = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName))
        Catch ex As System.IO.FileLoadException
        End Try

        If Not isSystem And toReturn Is Nothing Then
            ' don't bother if system, or if the GetType worked already (must be OK, surely?)
            Dim a As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(GetType(T))
            Dim assembly As String = a.FullName.Split(",")(0)
            If a Is Nothing Then
                Throw New ArgumentException("Assembly for type '" + GetType(T).Name.ToString() + "' could not be loaded.")
            Else
                'Dim newtype As Type = a.GetType(assembly + "." + className)
                Dim newtype As Type = a.GetType(className)
                If newtype Is Nothing Then
                    Throw New ArgumentException("Type '" + typeName + "' could not be loaded from assembly '" + assembly + "'.")
                Else
                    toReturn = newtype
                End If
            End If
        End If
        Return toReturn
    End Function
End Class

<Serializable()> _
Public Class WsqImage2
    Private _XSize As Integer = 0
    Private _YSize As Integer = 0
    Private _XRes As Integer = 0
    Private _YRes As Integer = 0
    Private _PixelFormat As Integer = 0
    Private _Content As Byte()
    Public Property XSize() As Integer
        Get
            Return _XSize
        End Get
        Set(ByVal Value As Integer)
            _XSize = Value
        End Set
    End Property
    Public Property YSize() As Integer
        Get
            Return _YSize
        End Get
        Set(ByVal Value As Integer)
            _YSize = Value
        End Set
    End Property
    Public Property XRes() As Integer
        Get
            Return _XRes
        End Get
        Set(ByVal Value As Integer)
            _XRes = Value
        End Set
    End Property
    Public Property YRes() As Integer
        Get
            Return _YRes
        End Get
        Set(ByVal Value As Integer)
            _YRes = Value
        End Set
    End Property
    Public Property PixelFormat() As Integer
        Get
            Return _PixelFormat
        End Get
        Set(ByVal Value As Integer)
            _PixelFormat = Value
        End Set
    End Property
    Public Property Content() As Byte()
        Get
            Return _Content
        End Get
        Set(ByVal Value As Byte())
            _Content = Value
        End Set
    End Property
End Class