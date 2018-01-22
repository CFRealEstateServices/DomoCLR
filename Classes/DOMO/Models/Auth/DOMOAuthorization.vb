
Imports System.Collections.Generic
Imports System.Text
Imports System

Namespace DOMO_CLR
    Public Class DOMOAuthorization
        Sub New()

        End Sub

        Public Property DOMO_CLIENT_ID() As String
            Get
                Return m_DOMO_CLIENT_ID
            End Get
            Set
                m_DOMO_CLIENT_ID = Value
            End Set
        End Property
        Private m_DOMO_CLIENT_ID As String
        Public Property DOMO_CLIENT_SECRET() As String
            Get
                Return m_DOMO_CLIENT_SECRET
            End Get
            Set
                m_DOMO_CLIENT_SECRET = Value
            End Set
        End Property
        Private m_DOMO_CLIENT_SECRET As String
        Public Property OAuthAccessToken() As String
            Get
                Return m_OAuthAccessToken
            End Get
            Set
                m_OAuthAccessToken = Value
            End Set
        End Property
        Private m_OAuthAccessToken As String
    End Class
End Namespace