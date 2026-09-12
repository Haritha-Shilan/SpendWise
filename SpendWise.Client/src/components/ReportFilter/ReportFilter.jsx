import { useState } from "react";
import { reportPeriods } from "../../config/reportPeriods";
import "./ReportFilter.css";

function ReportFilter({
    initialPeriod = 4,
    onApply,
}) {
    const [period, setPeriod] = useState(initialPeriod);
    const [fromDate, setFromDate] = useState("");
    const [toDate, setToDate] = useState("");
    const [dateError, setDateError] = useState("");

    const isCustomRange = period === 5;

    const handlePeriodChange = (event) => {
        const value = Number(event.target.value);

        setPeriod(value);

        if (value !== 5) {
            setFromDate("");
            setToDate("");
        }
    };

    const handleApply = () => {
        if (period === 5) {
            if (!fromDate || !toDate) {
                setDateError("Please select both From and To dates.");
                return;
            }

            if (fromDate > toDate) {
                setDateError("From date cannot be later than To date.");
                return;
            }
        }

        setDateError("");

        const selectedPeriod = reportPeriods.find(
            (item) => item.value === period
        );

        let periodLabel = selectedPeriod?.label || "";

        if (period === 5) {
            const from = new Date(fromDate).toLocaleDateString("en-GB", {
                day: "2-digit",
                month: "short",
                year: "numeric",
            });

            const to = new Date(toDate).toLocaleDateString("en-GB", {
                day: "2-digit",
                month: "short",
                year: "numeric",
            });

            periodLabel = `${from} – ${to}`;
        }

        onApply({
            period,
            periodLabel,
            fromDate: period === 5 ? fromDate : null,
            toDate: period === 5 ? toDate : null,
        });
    };

    return (
        <div className="report-filter">
            <div className="report-filter-field">
                <label htmlFor="report-period">
                    Period
                </label>

                <select
                    id="report-period"
                    value={period}
                    onChange={handlePeriodChange}
                >
                    {reportPeriods.map((item) => (
                        <option
                            key={item.value}
                            value={item.value}
                        >
                            {item.label}
                        </option>
                    ))}
                </select>
            </div>

            {isCustomRange && (
                <>
                    <div className="report-filter-field">
                        <label htmlFor="report-from-date">
                            From
                        </label>

                        <input
                            id="report-from-date"
                            type="date"
                            value={fromDate}
                            onChange={(event) =>
                                setFromDate(event.target.value)
                            }
                        />
                    </div>

                    <div className="report-filter-field">
                        <label htmlFor="report-to-date">
                            To
                        </label>

                        <input
                            id="report-to-date"
                            type="date"
                            value={toDate}
                            onChange={(event) =>
                                setToDate(event.target.value)
                            }
                        />
                    </div>
                    {dateError && (
                        <p className="report-filter-error">
                            {dateError}
                        </p>
                    )}
                </>
            )}

            <button
                type="button"
                className="report-filter-apply"
                onClick={handleApply}
            >
                Apply
            </button>
        </div>
    );
}

export default ReportFilter;