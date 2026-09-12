import { useEffect, useState } from "react";
import ReportFilter from "../../components/ReportFilter/ReportFilter";
import useUserReports from "../../hooks/useUserReports";
import "./ReportsPage.css";
import SummaryCard from "../../components/Dashboard/SummaryCard";
import { FaRupeeSign } from "react-icons/fa";
import { FiArrowDownLeft, FiArrowUpRight } from "react-icons/fi";
import MonthlySummary from "../../components/Reporting/MonthlySummary";
import ExpenseByCategory from "../../components/Reporting/ExpenseByCategory";
import ReportSummary from "../../components/Reporting/ReportSummary";
import IncomeSummary from "../../components/Reporting/IncomeSummary";

function ReportsPage() {
    const { report, isLoading, error, loadReport } = useUserReports();

    const [selectedPeriodLabel, setSelectedPeriodLabel] =
        useState("This Month");

    const initialFilter = {
        period: 1,
        fromDate: null,
        toDate: null,
    };

    useEffect(() => {
        loadReport(initialFilter);
    }, []);

    const handleFilterApply = (filter) => {
        setSelectedPeriodLabel(filter.periodLabel);
        loadReport(filter);
    };

    return (
        <div className="reports-page">
            <div className="reports-header">
                <h2>My Reports</h2>
                <p>Analyze your income and expenses.</p>
            </div>

            <ReportFilter
                initialPeriod={1}
                onApply={handleFilterApply}
            />

            {isLoading && <p>Loading report...</p>}

            {error && <p>{error}</p>}

            {report && (
                <>
                    <div className="reports-summary-grid">
                        <SummaryCard
                            title="Balance"
                            value={`₹${report.balance.toLocaleString("en-IN")}`}
                            description={selectedPeriodLabel}
                            icon={FaRupeeSign}
                            variant="balance"
                        />

                        <SummaryCard
                            title="Income"
                            value={`₹${report.totalIncome.toLocaleString("en-IN")}`}
                            description={selectedPeriodLabel}
                            icon={FiArrowDownLeft}
                            variant="income"
                        />

                        <SummaryCard
                            title="Expenses"
                            value={`₹${report.totalExpense.toLocaleString("en-IN")}`}
                            description={selectedPeriodLabel}
                            icon={FiArrowUpRight}
                            variant="expense"
                        />
                    </div>

                    <div className="reports-analysis-grid">
                        <MonthlySummary
                            data={report.monthlySummary}
                            subtitle={`Monthly Overview-${selectedPeriodLabel}`}
                        />

                        <ExpenseByCategory
                            data={report.expenseByCategory}
                            title="Expenses by Category"
                            subtitle={selectedPeriodLabel}
                            variant="pie"
                        />
                    </div>

                    <div className="reports-bottom-grid">
                        <IncomeSummary
                            data={report.incomeSummary}
                            subtitle={`Income sources for ${selectedPeriodLabel}`}
                        />

                        <ReportSummary
                            totalIncome={report.totalIncome}
                            totalExpense={report.totalExpense}
                            balance={report.balance}
                            expenseRatio={report.expenseRatio}
                            subtitle={`Quick view of  ${selectedPeriodLabel}`}
                        />
                    </div>

                </>
            )}
        </div>
    );
}

export default ReportsPage;