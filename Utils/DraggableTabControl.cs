using System.Windows.Forms;

namespace SqlQueryTool.Utils
{
    public class DraggableTabControl : TabControl
    {
        private TabPage predraggedTab;

        public DraggableTabControl()
        {
            AllowDrop = true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && predraggedTab != null)
                DoDragDrop(predraggedTab, DragDropEffects.Move);

            base.OnMouseMove(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            var draggedTab = (TabPage) drgevent.Data.GetData(typeof(TabPage));
            var pointedTab = getPointedTab();

            if (draggedTab == predraggedTab && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;

                if (pointedTab != draggedTab)
                    swapTabPages(draggedTab, pointedTab);
            }

            base.OnDragOver(drgevent);
        }

        private TabPage getPointedTab()
        {
            for (var i = 0; i < TabPages.Count; i++)
                if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
                    return TabPages[i];

            return null;
        }

        private void swapTabPages(TabPage src, TabPage dst)
        {
            var srci = TabPages.IndexOf(src);
            var dsti = TabPages.IndexOf(dst);

            TabPages[dsti] = src;
            TabPages[srci] = dst;

            if (SelectedIndex == srci)
                SelectedIndex = dsti;
            else if (SelectedIndex == dsti)
                SelectedIndex = srci;

            Refresh();
        }
    }
}