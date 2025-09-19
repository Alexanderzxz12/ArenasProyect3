using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArenasProyect3.Modulos.Resourses
{
    public class InactivityMonitor
    {
        private DateTime _lastActivity;
        private Panel _panel;
        private Timer _timer;
        private readonly Form _form;
        private readonly double _warningMinutes;
        private readonly double _shutdownMinutes;
        private bool _warningShown;

        public InactivityMonitor(Form form, Panel panelNotificacion, double warningMinutes = 3, double shutdownMinutes = 5)
        {
            _form = form;
            _warningMinutes = warningMinutes;
            _shutdownMinutes = shutdownMinutes;
            _lastActivity = DateTime.Now;
            _warningShown = false;
            _panel = panelNotificacion;

            _timer = new Timer { Interval = 1000 };
            _timer.Tick += CheckInactivity;
            _timer.Start();

            HookActivityEvents(_form);
        }

        private void CheckInactivity(object sender, EventArgs e)
        {
            double inactiveMinutes = (DateTime.Now - _lastActivity).TotalMinutes;

            // Mostrar advertencia solo una vez
            if (!_warningShown && inactiveMinutes >= _warningMinutes && inactiveMinutes < _shutdownMinutes)
            {
                _warningShown = true; // ✅ Marcar antes de mostrar
                _timer.Stop();        // ⛔ Detener el timer temporalmente

                _panel.Visible = true;
                //MessageBox.Show("Advertencia: el sistema se cerrará en 2 minutos si no hay actividad.", "Inactividad", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                _timer.Start();       // ✅ Reanudar el timer después del mensaje
            }

            // Cierre definitivo
            if (_warningShown && inactiveMinutes >= _shutdownMinutes)
            {
                _timer.Stop();
                //MessageBox.Show("Cerrando por inactividad...");
                Application.Exit();
            }
        }


        private void RegisterActivity(object sender, EventArgs e)
        {
            _lastActivity = DateTime.Now;

            // Solo reinicia la advertencia si ya fue mostrada
            if (_warningShown)
                _warningShown = false;
        }

        private void HookActivityEvents(Control control)
        {
            control.MouseMove += RegisterActivity;
            control.KeyPress += RegisterActivity;
            control.Click += RegisterActivity;

            foreach (Control child in control.Controls)
            {
                HookActivityEvents(child);
            }
        }
    }
}
