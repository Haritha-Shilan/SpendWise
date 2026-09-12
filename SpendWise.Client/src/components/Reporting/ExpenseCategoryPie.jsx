import {
    Cell,
    Legend,
    Pie,
    PieChart,
    ResponsiveContainer,
    Tooltip,
} from "recharts";

import "./ExpenseCategoryPie.css";

const pieColors = [
    "#7d7ae0",
    "#e79a8f",
    "#8db9a4",
    "#d8b570",
    "#9a9de8",
];

function ExpenseCategoryPie({ data = [] }) {
    const total = data.reduce(
        (sum, item) => sum + item.amount,
        0
    );

    return (
        <div className="expense-category-pie">
            <ResponsiveContainer width="100%" height={280}>
                <PieChart>
                    <Pie
                        data={data}
                        dataKey="amount"
                        nameKey="categoryName"
                        cx="38%"
                        cy="50%"
                        outerRadius={105}
                        stroke="none"
                    >
                        {data.map((item, index) => (
                            <Cell
                                key={item.categoryName}
                                fill={
                                    pieColors[
                                    index % pieColors.length
                                    ]
                                }
                            />
                        ))}
                    </Pie>

                    <Tooltip
                        formatter={(value) =>
                            `₹${Number(value).toLocaleString("en-IN")}`
                        }
                    />

                    <Legend
                        layout="vertical"
                        align="right"
                        verticalAlign="middle"
                        formatter={(value, entry) => {
                            const amount =
                                entry.payload.amount;

                            const percentage =
                                total > 0
                                    ? Math.round(
                                        (amount / total) * 100
                                    )
                                    : 0;

                            return `${value} — ${percentage}%`;
                        }}
                    />
                </PieChart>
            </ResponsiveContainer>
        </div>
    );
}

export default ExpenseCategoryPie;