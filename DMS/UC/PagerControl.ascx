<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PagerControl.ascx.cs" Inherits="DMS.UC.PagerControl" %>

<style type="text/css">
    .divPagerControl {
        width: 100%;
        text-align: center;
    }

        .divPagerControl .PagerOptions {
            display: inline-block;
        }

        .divPagerControl .PagerNumbers {
            display: inline-block;
            margin-left: 50px;
        }

        .divPagerControl #txtRowCount {
            border: 1px solid #ddd;
            border-radius: 3px;
            padding: 5px 10px;
            color: #666;
            background: rgba(0,0,0,0.1);
            cursor: pointer;
        }

        .divPagerControl #txtPageCount {
            color: blue;
            font-weight: bold;
        }

        .divPagerControl #txtPageSize, .divPagerControl #txtPageIndex {
            min-width: 30px;
            padding: 5px 10px;
            border: 1px solid #dddee1;
            border-radius: 3px;
            text-align: center;
        }

        .divPagerControl #txtPageIndex {
            max-width: 50px;
        }

        .divPagerControl .PageNumber {
            margin-right: 1px;
            padding: 5px 10px;
            border: 1px solid #ddd;
            border-radius: 3px;
            color: #666;
            background: rgba(0,0,0,0);
            cursor: pointer;
        }

            .divPagerControl .PageNumber:hover {
                color: white;
                background: #5d78ff;
                border: rgba(62,119,208,1) 1px solid;
            }

            .divPagerControl .PageNumber[disabled=disabled] {
                color: red;
                background: rgba(0,0,0,0.1);
                font-weight: bold;
            }

                .divPagerControl .PageNumber[disabled=disabled]:hover {
                    color: red;
                    border: 1px solid #ddd;
                    font-weight: bold;
                    cursor: default;
                }

        .divPagerControl .PageMore {
            border: 1px solid #ddd;
            border-radius: 3px;
            padding: 5px 10px;
            color: #666;
            background: rgba(0,0,0,0);
            cursor: pointer;
        }

            .divPagerControl .PageMore[disabled=disabled] {
                background: rgba(0,0,0,0.1);
            }

            .divPagerControl .PageMore:hover {
                color: white;
                background: #5d78ff;
                border: rgba(62,119,208,1) 1px solid;
            }

            .divPagerControl .PageMore[disabled=disabled]:hover {
                color: #666;
                background: rgba(0,0,0,0.1);
                border: 1px solid #ddd;
                cursor: default;
            }
</style>

<div id="divPagerControl" class="divPagerControl" runat="server">
    <div class="PagerOptions">
        <span>共<asp:Label ID="txtRowCount" Text="0" runat="server" />项</span>
        <span>每页<asp:DropDownList ID="txtPageSize" AutoPostBack="true" OnSelectedIndexChanged="PagerChange" runat="server">
            <asp:ListItem Text="10" Value="10" />
            <asp:ListItem Text="20" Value="20" />
            <asp:ListItem Text="30" Value="30" />
            <asp:ListItem Text="50" Value="50" />
        </asp:DropDownList>项</span>
        <span style="display: none;">分<asp:Label ID="txtPageCount" Text="0" runat="server" />页</span>
        <span>第<asp:TextBox ID="txtPageIndex" Text="1" AutoPostBack="true" OnTextChanged="PagerChange" runat="server" />页</span>
    </div>
    <div class="PagerNumbers">
        <asp:Button ID="btnPageFirst" CssClass="PageMore" Text="第一页" Enabled="false" OnClick="PagerChange" runat="server" />
        <asp:Button ID="btnPagePrevious" CssClass="PageMore" Text="上一页" Enabled="false" OnClick="PagerChange" runat="server" />
        <asp:HiddenField ID="txtPageNumberCount" Value="7" runat="server" />
        <asp:Repeater ID="divPageNumbers" runat="server" />
        <asp:Button ID="btnPageNext" CssClass="PageMore" Text="下一页" Enabled="false" OnClick="PagerChange" runat="server" />
        <asp:Button ID="btnPageLast" CssClass="PageMore" Text="最后一页" Enabled="false" OnClick="PagerChange" runat="server" />
    </div>
</div>
