<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ObjectDataSource ID="ObjectDataSource1"
        DataObjectTypeName="Ziri.MDL.UserInfo"
        TypeName="Ziri.Bll.UserManager"
        SelectMethod="GetPageList"
        SelectCountMethod="GetTotalRowsCount"
        EnablePaging="true"
        MaximumRowsParameterName="RowIndex"
        StartRowIndexParameterName="PageSize"
        runat="server"></asp:ObjectDataSource>
    <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
        <AlternatingItemTemplate>
            <tr>
                <th>
                    <asp:Label ID="CIDLabel" runat="server" Text='<%#Eval("ID")%>' />
                </th>
                <td>
                    <asp:Label ID="CNameLabel" runat="server" Text='<%#Eval("Name")%>' /></td>
                <td>
                    <asp:Label ID="CCreateionTime" runat="server" Text='<%#Eval("CreationTime")%>' /></td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                </td>
            </tr>
        </AlternatingItemTemplate>
        <EditItemTemplate>
            <tr>
                <th>
                    <asp:TextBox ID="CIDTextBox" runat="server" Text='<%#Bind("ID")%>' />
                </th>
                <td>
                    <asp:TextBox ID="CNameTextBox" runat="server" Text='<%#Bind("Name")%>' />
                </td>
                <td>
                    <asp:TextBox ID="CCreationTimeTextBox" runat="server" Text='<%#Bind("CreationTime")%>' />
                </td>
                <td>
                    <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="更新" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="取消" />
                </td>
            </tr>
        </EditItemTemplate>
        <EmptyDataTemplate>
            没有数据
        </EmptyDataTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:Button ID="InsertButton" runat="server" CommandName="Insert" Text="插入" />
                    <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="清除" />
                </td>
                <td>
                    <asp:TextBox ID="CIDTextBox" runat="server" Text='<%#Bind("ID")%>' />
                </td>
                <td>
                    <asp:TextBox ID="CNameTextBox" runat="server" Text='<%#Bind("Name")%>' />
                </td>
                <td>
                    <asp:TextBox ID="CCreationTimeTextBox" runat="server" Text='<%#Bind("CreationTime")%>' />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr>
                <th>
                    <asp:Label ID="CIDLabel" runat="server" Text='<%#Eval("ID")%>' />
                </th>
                <td>
                    <asp:Label ID="CNameLabel" runat="server" Text='<%#Eval("Name")%>' /></td>
                <td>
                    <asp:Label ID="CCreateionTime" runat="server" Text='<%#Eval("CreationTime")%>' /></td>
                <td>
                    <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                    <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                </td>
            </tr>
        </ItemTemplate>

        <SelectedItemTemplate>
            <tr>
                <th>
                    <asp:Label ID="CIDLabel" runat="server" Text='<%#Eval("ID")%>' />
                </th>
                <td>
                    <asp:Label ID="CNameLabel" runat="server" Text='<%#Eval("Name")%>' /></td>
                <td>
                    <asp:Label ID="CCreateionTime" runat="server" Text='<%#Eval("CreationTime")%>' /></td>
                <td>
                        <asp:Button ID="DeleteButton" runat="server" CommandName="Delete" Text="删除" />
                        <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="编辑" />
                    </td>
            </tr>
        </SelectedItemTemplate>




        <LayoutTemplate>
            <table id="itemPlaceholderContainer" runat="server">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Name</th>
                        <th>CreationTime</th>
                    </tr>
                </thead>
                <tbody>
                    <tr id="itemPlaceholder" runat="server"></tr>
                </tbody>
            </table>

            <asp:DataPager ID="DataPager1" PagedControlID="ListView1" PageSize="5" runat="server">
                <Fields>
                    <asp:TemplatePagerField>
                        <PagerTemplate>
                            <span>共</span><asp:Label runat="server" Text="<%#Container.TotalRowCount%>" /><span>项</span>
                            <span>每页</span><asp:Label runat="server" Text="<%#Container.PageSize%>" /><span>项</span>
                            <span>分</span><asp:Label runat="server" Text="<%#Math.Ceiling((double)Container.TotalRowCount/Container.PageSize)%>" /><span>页</span>
                        </PagerTemplate>
                    </asp:TemplatePagerField>
                    <asp:NumericPagerField />
                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowLastPageButton="True" />
                </Fields>
            </asp:DataPager>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>

