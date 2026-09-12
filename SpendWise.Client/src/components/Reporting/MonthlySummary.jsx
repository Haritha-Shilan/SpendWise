import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    Legend,
    ResponsiveContainer,
} from "recharts";

import "./MonthlySummary.css";

function MonthlySummary({
    data = [],
    title = "Income & Expenses",
    subtitle = "Monthly overview",
}) {
    const chartData = data.map((item) => ({
        ...item,
        displayMonth:
            data.some((x) => x.year !== item.year)
                ? `${item.monthName} '${String(item.year).slice(-2)}`
                : item.monthName,
    }));


    return (
        <div className="monthly-summary">
            <div className="monthly-summary-header">
                <div>
                    <h3>{title}</h3>
                    <p>{subtitle}</p>
                </div>
            </div>

            <div className="monthly-summary-chart">
                <ResponsiveContainer width="100%" height={260}>
                    <BarChart
                        data={chartData}
                        margin={{
                            top: 10,
                            right: 10,
                            left: 0,
                            bottom: 5,
                        }}
                        barCategoryGap="30%"
                    >
                        <CartesianGrid
                            vertical={false}
                            stroke="#e8ebf2"
                        />

                        <XAxis
                            dataKey="displayMonth"
                            axisLine={false}
                            tickLine={false}
                            tick={{
                                fill: "#7b8498",
                                fontSize: 11,
                            }}
                        />

                        <YAxis
                            axisLine={false}
                            tickLine={false}
                            tick={{
                                fill: "#7b8498",
                                fontSize: 11,
                            }}
                        />

                        <Tooltip
                            contentStyle={{
                                border: "1px solid #e8ebf2",
                                borderRadius: "8px",
                                boxShadow: "0 6px 20px rgba(23, 32, 51, 0.08)",
                                fontSize: "12px",
                            }}
                        />

                        <Legend
                            verticalAlign="bottom"
                            align="left"
                            iconType="circle"
                            wrapperStyle={{
                                paddingTop: "10px",
                                fontSize: "12px",
                                color: "#7b8498",
                            }}
                        />

                        <Bar
                            dataKey="income"
                            name="Income"
                            fill="#8c8ff0"
                            radius={[5, 5, 0, 0]}
                            barSize={14}
                        />

                        <Bar
                            dataKey="expense"
                            name="Expense"
                            fill="#e79aa5"
                            radius={[5, 5, 0, 0]}
                            barSize={14}
                        />
                    </BarChart>
                </ResponsiveContainer>
            </div>
        </div>
    );
}

export default MonthlySummary;