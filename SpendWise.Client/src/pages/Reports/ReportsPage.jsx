import ReportFilter from "../../components/ReportFilter/ReportFilter";
import "./ReportsPage.css";

function ReportsPage() {
    return (
        <div className="reports-page">
            <div className="reports-header">
                <div>
                    <h2>My Reports</h2>
                    <p>Analyze your income and expenses.</p>

                    <ReportFilter/>
                </div>
            </div>
        </div>
    );
}

export default ReportsPage;