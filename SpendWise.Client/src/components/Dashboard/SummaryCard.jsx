import './SummaryCard.css'

function SummaryCard({ title, value, description, icon: Icon, variant = "default", }) {
    return (
        <div className={`summary-card summary-card-${variant}`}>
            <div className="summary-card-content">
                <p className="summary-card-title">
                    {title}
                </p>

                <h3 className="summary-card-value">
                    {value}
                </h3>

                <p className="summary-card-description">
                    {description}
                </p>
            </div>

            <div className="summary-card-icon">
                {Icon && <Icon />}
            </div>
        </div>
    );
}

export default SummaryCard;