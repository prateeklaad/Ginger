#region License
/*
Copyright © 2014-2018 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion

using Ginger.Actions;
using Ginger.Repository;
using GingerWPF.UserControlsLib.UCTreeView;
using GingerCore.Actions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Ginger.SolutionWindows.TreeViewItems
{
    class SharedActionTreeItem : TreeViewItemBase, ITreeViewItem
    {
        private ActionEditPage mActionEditPage;
        public Act Act { get; set; }
        private SharedActionsFolderTreeItem.eActionsItemsShowMode mShowMode;
        public SharedActionTreeItem(SharedActionsFolderTreeItem.eActionsItemsShowMode showMode = SharedActionsFolderTreeItem.eActionsItemsShowMode.ReadWrite)
        {
            mShowMode = showMode;
        }

        Object ITreeViewItem.NodeObject()
        {
            return Act;
        }
        override public string NodePath()
        {
            return Act.FileName;
        }
        override public Type NodeObjectType()
        {
            return typeof(Act);
        }

        StackPanel ITreeViewItem.Header()
        {
            return TreeViewUtils.CreateLinkedItemHeader(Act, Act.Fields.Description, "@Flash_16x16.png", Ginger.SourceControl.SourceControlIntegration.GetItemSourceControlImage(Act.FileName, ref ItemSourceControlStatus));
        }

        List<ITreeViewItem> ITreeViewItem.Childrens()
        {
            return null;
        }

        bool ITreeViewItem.IsExpandable()
        {
            return false;
        }

        Page ITreeViewItem.EditPage()
        {
            if (mActionEditPage == null)
            {
                mActionEditPage = new ActionEditPage(Act, General.RepositoryItemPageViewMode.SharedReposiotry);
            }
            return mActionEditPage;
        }

        ContextMenu ITreeViewItem.Menu()
        {
            return mContextMenu;
        }

        void ITreeViewItem.SetTools(ITreeView TV)
        {
            mTreeView = TV;
            mContextMenu = new ContextMenu();
            if (mShowMode == SharedActionsFolderTreeItem.eActionsItemsShowMode.ReadWrite)
            {
                AddItemNodeBasicManipulationsOptions(mContextMenu);

                TreeViewUtils.AddMenuItem(mContextMenu, "View Repository Item Usage", ShowUsage, null, "@Link_16x16.png");

                AddSourceControlOptions(mContextMenu);
            }
            else
            {
                TreeViewUtils.AddMenuItem(mContextMenu, "View Repository Item Usage", ShowUsage, null, "@Link_16x16.png");
            }
        }       

        private void ShowUsage(object sender, RoutedEventArgs e)
        {
            RepositoryItemUsagePage usagePage = new RepositoryItemUsagePage(Act);
            usagePage.ShowAsWindow();
        }

        public override bool DeleteTreeItem(object item, bool deleteWithoutAsking = false, bool refreshTreeAfterDelete = true)
        {
            if (LocalRepository.CheckIfSureDoingChange(Act, "delete") == true)
            {
                return (base.DeleteTreeItem(Act, deleteWithoutAsking, refreshTreeAfterDelete));                
            }
            return false;
        }       
    }
}
